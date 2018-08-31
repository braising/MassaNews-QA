(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {
    //DROPDOWN e SUB-DROPDOWN
    $('.trigger-menu-dropdown').click(function(e){
      if ($(window).width() <= 456){
        e.preventDefault();

        var check = $(this).hasClass('is-open');

        //se o item já está aberto, apenas fecha
        if (check) {
          $(this).find('.fa-icon').removeClass('fa-angle-up').addClass('fa-angle-down');
          $(this).removeClass('is-open').next().slideUp('5000', function(){
            //Recalcular o scrollbar
            $(".c-nav-dropdown__list .nano").nanoScroller();
          });
        }

        //senão, fecha todos e exibi o selecionado
        else {
          // Fecha os itens abertos
          $(this).parent().parent().find('.is-open').removeClass('is-open').next().slideUp('2000');

          // Altera o ícone de todas as setas
          $(this).parent().parent().find('.fa-icon').removeClass('fa-angle-up').addClass('fa-angle-down');

          // Altera a seta selecionada
          $(this).find('.fa-icon').removeClass('fa-angle-down').addClass('fa-angle-up');

          $(this).addClass('is-open').next().slideDown('5000', function(){
            //Recalcular o scrollbar
            $(".c-nav-dropdown__list .nano").nanoScroller();
          });
        }
      }

    });

  });
})();
