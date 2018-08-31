//ESTRUTURA
  //
  //.c-accordion
  //  .c-accordion__module is-collapsed
  //    .c-accordion__tab
  //    .c-accordion__tabpanel

(function () {
  'use strict';
  window.addEventListener('DOMContentLoaded', function () {
    (function () {
      SetOpenAllFirstAccordions();
    })();
  });
})();

//Encontra o primeiro item sanfona de cada post e deixa aberto
function SetOpenAllFirstAccordions() {
  if ($(window).width() > 892) {
    var objPosts = $('.c-post');

    $('.c-post').each(function (index, objPost) {
      SetFirstItemOpen($(objPost));
    });
  }
}

//Seta o primeiro item do accordion open
function SetFirstItemOpen(objPost) {
  var firstPostFirstCarrocel = objPost.find('.c-post__sidebar').find('.c-accordion__module:first-of-type');
  firstPostFirstCarrocel.addClass('is-collapsed');
  firstPostFirstCarrocel.find('.c-accordion__tabpanel').show();
  accordionClick(objPost);
}

//Evento de click do accordion panel
function accordionClick(objPost) {
  objPost.find('.c-accordion__tab').click(function (e) {
    e.preventDefault();

    var check = $(this).parent().hasClass('is-collapsed');
    if (check) {
      //abre a sanfona clicada
      $(this).parent().removeClass('is-collapsed');
      $(this).next().slideUp('300');
    }
    else {
      //fecha todas as sanfonas do grupo especifico
      var accordionRef = $(this).parent().parent();
      accordionRef.find('.c-accordion__module').removeClass('is-collapsed');
      accordionRef.find('.c-accordion__tabpanel').slideUp('300');

      //abre a sanfona clicada
      $(this).parent().addClass('is-collapsed');
      $(this).next().slideDown('300');
    }
  });
}