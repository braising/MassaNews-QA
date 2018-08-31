(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {
    //deixar a primeira aba de cada conjunto com a classe is-active
    //$('.c-tabs__nav .c-tabs__tab').first().addClass('is-active');

    $('.c-tabs__tab').click(function(e){
      //cancelar qualquer açaão do botão/link
      e.preventDefault();

      //de qual conjunto de abas pertence
      var tabs = $(this).attr("data-tabs");

      //qual painel mostrar
      var tabpanel = $(this).attr("data-tabpanel");

      if(tabs && tabpanel) {
        //remover todas as classes is-active das abas do conjuto
        $('.c-tabs[data-tabs="' + tabs + '"] .c-tabs__tab').removeClass('is-active');

        //adicionar a classe is-active na aba clicada
        $(this).addClass('is-active');

        //ocultar todos os paineis do conjuto especifico de abas
        $('#' + tabs + ' .c-tabs__tabpanel').addClass('u-none').removeClass('u-block');

        //exibir o painel espeficico
        $('#' + tabpanel).addClass('u-block').removeClass('u-none');
      }

      //verificar se a aba é do sidebar
      if(tabs == 'tab-sidebar') {
        $(".c-fixed-sidebar .nano").nanoScroller();
      }
    });


    $('.trigger-more-tabs').click(function(e){
      var check = $(this).hasClass('is-open');

      if (check) {
        $(this).removeClass('is-open').addClass('is-hidden').find('.fa-caret-up').removeClass('fa-caret-up').addClass('fa-caret-down');
        $(this).removeClass('is-open').addClass('is-hidden');
        $('.more-tabs').removeClass('is-open').addClass('is-hidden');
      } else {
        $(this).removeClass('is-hidden').addClass('is-open');
        $(this).removeClass('is-hidden').addClass('is-open').find('.fa-caret-down').removeClass('fa-caret-down').addClass('fa-caret-up');
        $('.more-tabs').removeClass('is-hidden').addClass('is-open');
      }
      e.preventDefault();
    });


    // TABS
    $(document).on('click', '.trigger-tabs-tab', function (e) {
      //cancelar qualquer açaão do botão/link
      e.preventDefault();

      //de qual conjunto de abas pertence
      var tabs = $(this).attr("data-tabs");

      //qual painel mostrar
      var tabpanel = $(this).attr("data-tabpanel");

      if (tabs && tabpanel){
        //remover todas as classes is-active das abas do conjuto
        $('.c-category--nav li').removeClass('active');

        //adicionar a classe is-active na aba clicada
        $(this).parent().addClass('active');

        //ocultar todos os paineis do conjuto especifico de abas
        $('#' + tabs + ' .c-tabs__tabpanel').addClass('u-none').removeClass('u-block');

        //exibir o painel espeficico
        $('#' + tabpanel).addClass('u-block').removeClass('u-none');

        // SLIDER
        // Desabilita todos os sliders encontrados
        if ($(window).width() <= 1300) {
          $('#' + tabpanel).parent().find('div[class*=c-slider]').slick("unslick");
        }

        // Habilita o slider clicado
        $('#' + tabpanel).find('div[class*=c-slider]').slick('reinit');
      }

      //verificar se a aba é do sidebar
      if (tabs == 'tab-sidebar') {
        $(".c-fixed-sidebar .nano").nanoScroller();
      }

      //$("img.lazy").lazyload();

      return false;
    });

  });
})();