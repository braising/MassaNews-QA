(function (window, $) {

  'use strict';

  $(document).ready(function(){
    var allBg = $('.c-nav__all-bg')[0];
    var body = $('body')[0];

    // MEGA MENU V2
    var menuEl = document.getElementById('ml-menu'),
      mlmenu = new MLMenu(menuEl, {
        breadcrumbsCtrl : false, // show breadcrumbs
        // initialBreadcrumb : 'all', // initial breadcrumb text
        backCtrl : true, // show back button
        itemsDelayInterval : 0, // delay between each menu item sliding animation
      });

    // mobile menu toggle
    var openMenuCtrl = document.querySelector('.action--open'),
      closeMenuCtrl = document.querySelector('.action--close');

    openMenuCtrl.addEventListener('click', openMenu);
    closeMenuCtrl.addEventListener('click', closeMenu);

    function openMenu() {
      classie.add(menuEl, 'menu--open');
      body.classList.add('is-no-scrolling');
      allBg.classList.add('is-show');
      trackShowNav();
    }

    function closeMenu() {
      classie.remove(menuEl, 'menu--open');
      body.classList.remove('is-no-scrolling');
      allBg.classList.remove('is-show');
      trackHideNav();
    }

    $(document).on('click', '.c-nav__all-bg', function() {
      closeMenu();
    });

    //Ocultar o menu ao clicar no modal dentro dele
    $(document).on('click', '.menu__home[rel=modal]', function() {
      closeMenu();
    });

    // Track GA
    function trackShowNav() {
      if(typeof dataLayer !== 'undefined') {
        dataLayer.push({
          'event' : 'open-menu'
        });
      }
    }

    function trackHideNav() {
      if(typeof dataLayer !== 'undefined') {
        dataLayer.push({
          'event' : 'close-menu'
        });
      }
    }

    // Mapa - Navegue por região
    $('.trigger-map-regiao').on('click touchend', function(e) {
      var el = $(this);
      var elRegiao = el.data('regiao');
      var linkRegiao = $('#map-regiao-' + elRegiao);
      linkRegiao[0].click();
    });

    // Remove click do link
    $(document).on('click', '.trigger-no-click', function (event) {
      event.preventDefault();
    });

    // Label com o nome das regiões
    if ($(window).width() > 890) {
      $('.trigger-map-regiao').mouseenter(function() {
        $(this).siblings('.text-' + $(this).data('regiao')).fadeIn();
      }).mouseleave(function() {
        $(this).siblings('.text-' + $(this).data('regiao')).fadeOut('fast');
      });
    }

    // Hack para click na seta (>)
    $(document).on('click', '#ml-menu .menu__link i.fa', function (event) {
      $(this).parent('a')[0].click();
      event.preventDefault();
    });

    // Track GA - Clicks Menu Navigation
    $('#ml-menu .menu__link').on('click', function(event){
      // Texto do link clicado
      var textClick = this.text;

      // Título dos links principais (Ex: Notícias, Esportes)
      var titlePrimary

      // Verifica se é um link principal (Ex: Notícias, Esportes)
      var isPrimary = $(this).hasClass('menu__link-primary');

      // Título do submenu ativo (.see-all) / (Ex: + Esportes)
      var titleCurrent = $('ul.menu__level--current .see-all').text();

      // Submenu ativo
      var dataMenuPrimaryValeu = $('ul.menu__level--current .see-all').parents('ul.menu__level--current').attr('data-menu');

      // Verifica se é um submenu
      var isLevelOne   = $(this).parents('ul.menu__level.level-1').attr('data-menu');
      var isLevelTwo   = $(this).parents('ul.menu__level.level-2').attr('data-menu');
      var isLevelThree = $(this).parents('ul.menu__level.level-3').attr('data-menu');

      // Submenu link (Ex: Link "+ Esportes, Gastronomia")
      var hasSubmenu = $(this).attr('data-submenu') && !isPrimary;
      var textSubmenuLink;

      // Links Redes Sociais
      var isSocialMedia = $(this).hasClass('menu__link-social-medias');

      // Links institucionais
      var isInstitucionais = $(this).hasClass('menu__link-institucionais');

      // Link navegável
      var hasLink = $(this).attr('href') !== '#';
      var textHasLink;

      // Submenu Level 1
      if (typeof isLevelOne !== "undefined"){
        var titlePrimary = $('#ml-menu').find('.menu__link-primary[data-submenu="' + dataMenuPrimaryValeu + '"]').text();
        var textLevelOne = titlePrimary + ' > ' + textClick;
        textSubmenuLink = textLevelOne;
        textHasLink = textLevelOne;
      }

      // Submenu Level 2
      if (typeof isLevelTwo !== "undefined"){
        var titlePrimary = $('#ml-menu').find('.menu__link-primary[data-submenu="' + dataMenuPrimaryValeu.slice(0,-2) + '"]').text();
        var titleCurrent = $('ul.menu__level--current .see-all').text();
        var textLevelTwo = titlePrimary + ' > ' + titleCurrent + ' > ' + textClick;
        textSubmenuLink = textLevelTwo;
        textHasLink = textLevelTwo;
      }

      // Submenu Level 3
      if (typeof isLevelThree !== "undefined"){
        var titlePrimary = $('#ml-menu').find('.menu__link-primary[data-submenu="' + dataMenuPrimaryValeu.slice(0,-4) + '"]').text();
        var titleLevelThree = $('#ml-menu').find("[data-menu='" + isLevelThree.slice(0,-2) + "']").find('.see-all').text();
        var textLevelThree = titlePrimary + ' > ' + titleLevelThree + ' > ' + titleCurrent + ' > ' + textClick;
        textSubmenuLink = textLevelThree;
        textHasLink = textLevelThree;
      }

      // Tracks
      if(typeof dataLayer !== 'undefined') {
        if(isPrimary){
          dataLayer.push({
            'event' : 'primary-link-menu-navigation',
            'label-menu-navigation' : textClick
          });
        } else if(hasSubmenu){
          dataLayer.push({
            'event' : 'secondary-link-menu-navigation',
            'label-menu-navigation' : textSubmenuLink
          });
        } else if(isSocialMedia) {
          dataLayer.push({
            'event' : 'links-social-medias-menu-navigation',
            'label-menu-navigation' : textClick
          });
        } else if(isInstitucionais) {
          dataLayer.push({
            'event' : 'links-institucionais-menu-navigation',
            'label-menu-navigation' : textClick
          });
        } else if(hasLink) {
          dataLayer.push({
            'event' : 'link-menu-navigation',
            'label-menu-navigation' : textHasLink
          });
        }
      }
    });

  });
})(window, jQuery);

// Track Go Back
function trackMenuBack (event) {
  var textBack = $(event).find('.see-all').text();
  var titleCurrentBack = $('ul.menu__level--current .see-all').text();
  var textGoBack = 'voltar para ' + textBack + ' // ' + titleCurrentBack;

  // Tracks
  if(typeof dataLayer !== 'undefined') {
    dataLayer.push({
      'event' : 'go-back-menu-navigation',
      'label-menu-navigation' : textGoBack
    });
  }
}