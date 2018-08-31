(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {
    //Adicionar icones para links externos
    var externalLinks = document.querySelectorAll('.is-external');

    for (var i = 0; i < externalLinks.length; i++) {
      externalLinks[i].innerHTML += '<span class="icon-external"></span>';
    }

  });

})();
