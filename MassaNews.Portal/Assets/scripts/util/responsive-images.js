(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {
    var imgSlider = document.querySelectorAll('.c-slider__img');
    var timer;

    window.addEventListener('resize', handleResponsiveImages.bind(null, imgSlider));

    handleResponsiveImages(imgSlider);
  });

  function handleResponsiveImages(images) {
    //images responsivas
    if (images.length > 0) {
      var newSrc = '';

      for (var i = 0; i < images.length; i++) {
        if (window.innerWidth > 860) {
          newSrc = $(images[i]).data('src-desktop');
        } else {
          newSrc = $(images[i]).data('src-mobile');
        }

        if (newSrc !== '') {
          images[i].src = newSrc;
        }

      }
    }
  }

})();
