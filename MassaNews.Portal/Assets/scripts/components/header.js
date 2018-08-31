(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {

    var page = $('body').hasClass('pg-home');

    if (page) {
      $('.c-header__bar').addClass('is-fat');

      (function () {
        var header = $('.c-header__bar');
        var main = $('.g-main');

        document.addEventListener('scroll', function () {
          if ($(document).scrollTop() > 80) {

            if(!header.hasClass('is-slim')) {
              $(header).addClass('is-slim').removeClass('is-fat').hide().slideDown(300);
              $(main).addClass('is-transposed');
            }
          } else {
            $(header).removeClass('is-slim').addClass('is-fat');
            $(main).removeClass('is-transposed');
          }
        });
      })();
    }

    //menu dropdown
    $('.c-nav-dropdown__title').click(function(){
      $(this).toggleClass('is-open');
      $('.c-nav-dropdown').slideToggle(300);

      // Altera o ícone
      var icon = $(this).find('.c-nav-dropdown__icon');
      if(icon.hasClass('fa-angle-down')){
        icon.removeClass('fa-angle-down').addClass('fa-angle-up');
      } else {
        icon.removeClass('fa-angle-up').addClass('fa-angle-down');
      }
    });

  });
})();
