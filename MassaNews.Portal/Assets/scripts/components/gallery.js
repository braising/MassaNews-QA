(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {
    /*
    * SLICK SLIDER GALLERY
    */
    $('.c-gallery .c-gallery__view').each(function(index, obj){
      SetGallery(obj);
    });

    /*
    * LIGHT GALLERY
    */
    // Abre a galeria ao clicar na imagem principal
    $(document).on("click", ".trigger-open-lightgallery", function (event) {
      $(this).siblings('.trigger-lightgallery').find('.lg-iten-1').click();
      event.preventDefault();
    });

    // Abre a galeria ao clicar na seta "proxima"
    $(document).on("click", ".trigger-open-next-lightgallery", function (event) {
      $(this).siblings('.trigger-lightgallery').find('.lg-iten-2').click();
      event.preventDefault();
    });

    // Inicia o plugin de galeria
    $('.trigger-lightgallery').each(function(index, obj){
      SetLightGallery(obj);
    });
  });

})();

function SetGallery(obj){
  $(obj).slick({
    adaptiveHeight: true,
    responsive: [ {
      breakpoint: 500,
      settings: { 
        dots: true, 
        arrows: false 
      }
    }]
  });
}

// Config
var showThumb;
if ($(window).width() <= 565){
  showThumb = false
} else {
  showThumb = true
}

function SetLightGallery(obj){
  $(obj).lightGallery({
    download: false,
    thumbWidth: 100,
    thumbContHeight: 80,
    nextHtml: '<i class="fa fa-10x fa-angle-right"></i>',
    prevHtml: '<i class="fa fa-10x fa-angle-left"></i>',
    showThumbByDefault: showThumb
  });

  // Track
  $(obj).on('onAfterOpen.lg',function(event){
    TrackLightGallery('open-gallery');
  });
}

function TrackLightGallery(event){
  if(typeof dataLayer !== 'undefined') {
    dataLayer.push({
      'event' : event
    });
  }
}