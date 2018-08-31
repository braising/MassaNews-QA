/* global $:true */

(function ()
{
  'use strict';

  // Document.ready
  window.addEventListener('DOMContentLoaded', function ()
  {
    // Update username
    var name = getCookie('username');
    if (name == "") {      
      $.ajax('/minha-conta/me')
        .success(function (data) {
          //console.log(data);
          if (data.Status === true) {
            var name = data.Name;
            setUsername(name);
            document.cookie = 'username=' + name;
          };
        })
        .error(function (data) {
          console.log("error: " + data)
        });
      // .always(function () {
      //   $('.minha-conta-topo').html(html_topo);
      //   $('.minha-conta-lateral').html(html_lateral);
      // });
    } else {
      setUsername(name);
    }

    // Initialize FastClick
    FastClick.attach(document.body);

    imgLazyLoad();

    // Mascara dos selects
    $('.mask-select').append('<span class="mask"></span>');
    $('.mask-select .mask').click(function(event){
      var mEvent = new MouseEvent('mousedown');
      $(this).parent().find('select')[0].dispatchEvent(mEvent);
    });

    //INPUT FILE
    //ao clicar na mascara, um click é executado no input file
    $('.mask-file .mask-file__view').click(function(){
      $('.mask-file input').trigger('click');
    });

    //exibir nome do arquivo no input file do curriculo
    $('.mask-file input').on('change', function () {
      var valorFile = $(this).val().replace("C:\\fakepath\\", "");
      if(valorFile){
        $('.mask-file__view .mask-file__value').html(valorFile);
      }
    });

    //SVGs
    jQuery('img.svg').each(function(){
      var $img = jQuery(this);
      var imgID = $img.attr('id');
      var imgClass = $img.attr('class');
      var imgURL = $img.attr('src');

      jQuery.get(imgURL, function(data) {
        // Get the SVG tag, ignore the rest
        var $svg = jQuery(data).find('svg');
        // Add replaced image's ID to the new SVG
        if(typeof imgID !== 'undefined') {
          $svg = $svg.attr('id', imgID);
        }
        // Add replaced image's classes to the new SVG
        if(typeof imgClass !== 'undefined') {
          $svg = $svg.attr('class', imgClass+' replaced-svg');
        }
        // Remove any invalid XML tags as per http://validator.w3.org
        $svg = $svg.removeAttr('xmlns:a');
        // Replace image with new SVG
        $img.replaceWith($svg);
      }, 'xml');
    });

    //scrollbar personalizado do menu principal
    //$(".c-nav__menu .nano").nanoScroller();

    //scrollbar personalizado do sidebar fixo
    $(".c-fixed-sidebar .nano").nanoScroller();

    //controles de ordenação - resultado da busca
    $('.c-category-title__controls .controls__btn').click(function(){
      $(this).parent().find('.controls__btn').removeClass('is-current');
      $(this).addClass('is-current');
    });

    //post__historic
    $('.button-historic').click(function(){
      $(this).toggleClass('is-open').parent().next().slideToggle('300');
    });
    $('.button-historic-close').click(function(){
      $(this).parent().slideToggle('300')
      .parent().find('.button-historic').toggleClass('is-open');
    });

    //select assunto - pagina 'fale conosco'
    $('.select-assunto').change(function(){
      var area = $(this).val();

      if(area == 'assunto3'){
        window.location.href = "envie-sua-noticia";
      }

      if(area) {
        $('.area-assuntos .institucinais__box').hide();
        $('#'+area).fadeIn(200);
      } else {
        $('.area-assuntos .institucinais__box').hide();
      }
    });

    //resumo do autor - pagina interna dos blogs
    $('.c-author-profile__dropdown').click(function(){
      $(this).toggleClass('is-show');
    });

    //desabilitar o scroll nos mapas
    // you want to enable the pointer events only on click;
    $('.map iframe').addClass('scrolloff'); // set the pointer events to none on doc ready
    $('.map').on('click', function () {
      $('.map iframe').removeClass('scrolloff'); // set the pointer events true on click
    });
    // you want to disable pointer events when the mouse leave the canvas area;
    $("map").mouseleave(function () {
      $('.map iframe').addClass('scrolloff'); // set the pointer events to none when mouse leaves the map area
    });

    //ao passar o meuse sobre o titulo do video, a imagem acima tbm fica com o efeito do hover
    //$('.c-headline').mouseenter(function() {
    $('.c-headline .image a, .c-headline .title').mouseenter(function() {
      var check = $(this).parent().parent().find('.image').attr('rel');
      var checkIsVideo = $(this).parent().parent().find('.image').hasClass('modal-video');

      if (check == 'modal' || checkIsVideo) {
      } else {
        $(this).parent().parent().addClass('is-hover');
      }
    }).mouseleave(function() {
      $(this).parent().parent().removeClass('is-hover');
    });

    //Carrega as informações do tempo
    //LoadCityWether();

    // voltar / goBack
    $(document).on('click', '.trigger-go-back', function (event) {
      goBack();
      event.preventDefault();
    });

    // VÍDEOS - Dropdown categoria nav
    $(document).on('click', '.trigger-category-dropdown', function (e) {
      if ($(window).width() <= 880){
        e.preventDefault();

        var check = $(this).hasClass('is-open');

        //se o item já está aberto, apenas fecha
        if (check) {
          $(this).find('.fa').removeClass('fa-angle-up').addClass('fa-angle-down');
          $(this).removeClass('is-open').next().slideUp('5000', function(){});
        }

        //senão, fecha todos e exibi o selecionado
        else {
          // Altera a seta selecionada
          $(this).find('.fa').removeClass('fa-angle-down').addClass('fa-angle-up');
          $(this).addClass('is-open').next().slideDown('5000', function(){});
        }
      }
    });

    // TICKER NEWS - HOME
    $.fn.ticker.defaults = {
      random:        false, // Whether to display ticker items in a random order
      itemSpeed:     3000,  // The pause on each ticker item before being replaced
      cursorSpeed:   50,    // Speed at which the characters are typed
      pauseOnHover:  true,  // Whether to pause when the mouse hovers over the ticker
      finishOnHover: true,  // Whether or not to complete the ticker item instantly when moused over
      cursorOne:     '',   // The symbol for the first part of the cursor
      cursorTwo:     '',   // The symbol for the second part of the cursor
      fade:          true,  // Whether to fade between ticker items or not
      fadeInSpeed:   300,   // Speed of the fade-in animation
      fadeOutSpeed:  300    // Speed of the fade-out animation
    };
    var tickerNews = $('.trigger-ticker');
    tickerNews.fadeIn('fast').ticker();


    // VÍDEOS RELACIONADOS - POST
    $(document).on('click', '.trigger-close-mask-video', function () {
      closeRelatedVideos();
    });

    $(document).on('click', '.trigger-open-mask-video', function () {
      openRelatedVideos();
    });

    function openRelatedVideos() {
      $('.c-post--active').find('.c-post__videos-related').fadeIn('fast');
      event.preventDefault();
    }

    function closeRelatedVideos() {
      $('.c-post--active').find('.c-post__videos-related').fadeOut('fast');
      event.preventDefault();
    }


    // FECHAR BANNER
    $(document).on('click', '.trigger-close-ad', function (e) {
      var idAd = $(this).parents('.inventory').attr('id'),
          date = new Date(),
          expires = 'expires=';

      date.setDate(date.getDate() + 1);
      expires += date.toUTCString();

      // Hidde ad
      $(this).parents('.c-publicidade').fadeOut('fast');

      // Create cookie
      document.cookie = "close-ad-" + idAd + "=" + idAd + "; path=/;" + expires;
      e.preventDefault();
    });


    // INIT DATE
    atualizarData();

    // MASK
    $('.trigger-mask-date').mask('00/00/0000');
    $('.trigger-mask-cpf').mask('000.000.000-00');

    var SPMaskBehavior = function (val) {
      return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
    spOptions = {
      onKeyPress: function(val, e, field, options) {
        field.mask(SPMaskBehavior.apply({}, arguments), options);
      }
    };
    $('.trigger-mask-phone').mask(SPMaskBehavior, spOptions);


    // Scroll element
    if($('.trigger-scroll').length) {
      $(document).on('click', '.trigger-scroll', function (event) {
        event.preventDefault();
        var element = $($(this).attr('href'));
        var headerBar = $('.c-header__bar');
        var headerBarHeight = headerBar.height() + 10;

        $('body,html').animate({scrollTop: element.offset().top - headerBarHeight});

        // if(headerBar.hasClass('is-fat')){
        //   $('body,html').animate({scrollTop: element.offset().top});
        // } else {
        //   $('body,html').animate({scrollTop: element.offset().top - headerBarHeight});
        // }
      });
    }

    // Scroll to comments
    if($('.trigger-scroll-comments').length) {
      $(document).on('click', '.trigger-scroll-comments', function (event) {
        event.preventDefault();
        var element = $(this).parents(".c-post").find(".c-post__comments");
        var headerBar = $('.c-header__bar');
        var headerBarHeight = headerBar.height() + 10;

        $('body,html').animate({scrollTop: element.offset().top - headerBarHeight});
      });
    }

    // scroll
    var scrollUrl = getUrlParameter('go');
    if(scrollUrl != null){

      console.log(scrollUrl);

      event.preventDefault();
      var element = $("#" + scrollUrl);
      var headerBar = $('.c-header__bar');
      var headerBarHeight = headerBar.height() + 10;

      $('body,html').animate({scrollTop: element.offset().top - headerBarHeight});
    }


  });

})();


// WINDOW LOAD
$(window).load(function(){

  // vars
  var loadBar     = $('.trigger-load-bar');
  var bannerFloat = $('.trigger-banner-float');

  // Show/Hide Ad
  if ($('.inventory').length) {
    $('.inventory').each(function(){
      if($(this).is(':visible')){
        // Add Label
        //$(this).addClass('inventory--label');

        // active fullpage
        $(this).parent('.banner-float--fullpage').addClass('banner-float--fixed-on');

        // Active close button
        $(this).find('.trigger-close-ad').removeClass('u-none');
      } else {
        $(this).parents('.c-publicidade').hide();
      }
    });
  }

  if (loadBar.length) {
    loadBar.addClass('load-bar').html('<div class="bar-animation"></div>');
  }

  if ($('.trigger-banner-float').length) {
    $('.trigger-banner-float').each(function(){
      $(this).delay(7000).fadeOut('fast');

      //console.log($(this).find('.inventory'));

      // var delay=10000, setTimeoutConst;

      // $(this).find('.inventory').on('hover', function() {
      //   setTimeoutConst = setTimeout(function(){
      //     $(this).fadeOut('fast');
      //     console.log('aaaaaaaaaaquiii');
      //   }, delay);
      // }, function(){
      //   console.log('opaaaa');
      //   clearTimeout(setTimeoutConst);
      // });


      // var timer;
      // var delay = 10000;
      // $(this).find('.inventory').hover(function() {
      //     // on mouse in, start a timeout
      //     console.log('entrou');
      //     timer = setTimeout(function() {
            
      //       $(this).addClass('paused-animation');
      //       $(this).parent().fadeOut('fast');
      //     }, delay);
      // }, function() {
      //     // on mouse out, cancel the timer
      //     console.log('saiu');
      //     clearTimeout(timer);
      // });


      // var timer = null;
      // function hideBanner() {
      //   console.log('ja era');
      //   $(this).parent().fadeOut('fast');
      // }

      // function startSetInterval() {
      //   timer = setInterval(hideBanner, 3000);
      // }

      // // start function on page load
      // startSetInterval();

      // // hover behaviour
      // $(this).find('.inventory').hover(function() {
      //   console.log('aqui');
      //   clearInterval(timer);
      // },function() {
      //   console.log('opa');
      //   startSetInterval();
      // });
    });
  }

  // if (bannerFloat.length) {
  //   bannerFloat.delay(10000).fadeOut('fast');
  // }
});



// History Back
function goBack() {
  window.history.back();
}

// Image LazyLoad
function imgLazyLoad() {
  var imgPlaceholder;

  if($('body').hasClass('pg-videos')){
    imgPlaceholder = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAAAAAA6fptVAAAACklEQVR42mMQAgAAFAATr5UBVgAAAABJRU5ErkJggg==";
  }

  $("img.lazy").lazyload({
    effect : "fadeIn",
    placeholder: imgPlaceholder
  });
}


// DATE
function construirArray(qtdElementos){
  this.length = qtdElementos
}

var arrayDia = new construirArray(7);
  arrayDia[0] = "Domingo";
  arrayDia[1] = "Segunda-Feira";
  arrayDia[2] = "Terça-Feira";
  arrayDia[3] = "Quarta-Feira";
  arrayDia[4] = "Quinta-Feira";
  arrayDia[5] = "Sexta-Feira";
  arrayDia[6] = "Sábado";

var arrayMes = new construirArray(12);
  arrayMes[0] = "Janeiro";
  arrayMes[1] = "Fevereiro";
  arrayMes[2] = "Março";    
  arrayMes[3] = "Abril";
  arrayMes[4] = "Maio";
  arrayMes[5] = "Junho";
  arrayMes[6] = "Julho";
  arrayMes[7] = "Agosto";
  arrayMes[8] = "Setembro";
  arrayMes[9] = "Outubro";
  arrayMes[10] = "Novembro";
  arrayMes[11] = "Dezembro";

function mostrarData(diaSemana, dia, mes, ano){
  retorno = diaSemana+", "+dia+" de "+mes+" de "+ano;
  document.getElementById("trigger-date").innerHTML = retorno;
}

function getMesExtenso(mes){
  return this.arrayMes[mes];
}

function getDiaExtenso(dia){
  return this.arrayDia[dia];
}

function atualizarData(){ 
  var dataAtual = new Date();
  var dia = dataAtual.getDate();
  var diaSemana = getDiaExtenso(dataAtual.getDay());
  var mes = getMesExtenso(dataAtual.getMonth());
  var ano = dataAtual.getFullYear();

  mostrarData(diaSemana, dia, mes, ano);
  setTimeout("atualizarData()", 300000);
} 

// GET URL PARAMETER
var getUrlParameter = function getUrlParameter(sParam) {
  var sPageURL = decodeURIComponent(window.location.search.substring(1)),
      sURLVariables = sPageURL.split('&'),
      sParameterName,
      i;

  for (i = 0; i < sURLVariables.length; i++) {
    sParameterName = sURLVariables[i].split('=');

    if (sParameterName[0] === sParam) {
      return sParameterName[1] === undefined ? true : sParameterName[1];
    }
  }
};

function getCookie(cname) {
  var name = cname + "=";
  var decodedCookie = decodeURIComponent(document.cookie);
  var ca = decodedCookie.split(';');
  for (var i = 0; i < ca.length; i++) {
    var c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

function setUsername(name) {
  var html_topo = '<div class="profile"><a href="/minha-conta"><i class="fa fa-user t-page-color hide-mobile"></i><span class="hide-mobile">Olá, </span>' + name + '<i class="fa fa-angle-down"></i></a></div>';
  var html_lateral = '<a href="/minha-conta" class="menu__account u-cut" title="Minha conta"><i class="fa fa-user"></i> &nbsp;Olá, ' + name + '</a>';

  $('.minha-conta-topo').html(html_topo);
  $('.minha-conta-lateral').html(html_lateral);
}