(function () {
  'use strict';

  window.addEventListener('DOMContentLoaded', function () {

    // Atualiza a home após 5 minutos
    var isHome = $('body').hasClass('pg-home');
    if(isHome){
      //var timeRefresh = 60000 * 5;
      var timeRefresh = 300000;
      var intervalRefresh = setInterval(refreshHome, timeRefresh);
    }

    function refreshHome()
    {
      location.reload();
    }

    function activeRefreshHome() {
      if(isHome){
        setInterval(refreshHome, timeRefresh);
      }
    }

    // Load the modal window
    //$('[rel=modal]').click(function (e){
    $(document).on('click', '[rel=modal]', function (e) {
      e.preventDefault();

      var ref       = $(this).attr('data-modal-ref');
      var href      = $(this).attr('data-modal-href');
      var title     = $(this).attr('data-modal-title');
      var imgs      = $(this).attr('data-modal-imgs');
      var desc      = $(this).attr('data-modal-desc');
      var check     = $(this).attr('data-modal');

      //IF YouTube
      var re = new RegExp('.*www.youtube.com\/embed\/.*');
      if (re.test(ref))
        ref += '?autoplay=1&rel=0';

      // VIDEO
      if ($(this).hasClass('modal-video'))
      {
        //insere o link do video dentro do player (iframe)
        $('#player').attr("src", ref);
        $('#modal-video .title').html(title);
        $('#modal-video .post-link a').attr("href", href).html(title);

        // Desativa atualização automática da home
        clearTimeout(intervalRefresh);
      }

      // GALERIA
      if ($(this).hasClass('modal-gallery'))
      {
        //console.log(arrayImgs.length);

        // TEMPLATE
        //
        //<div class="iten">
        //  <img src="[imgs]" alt="[desc]">
        //
        //  <div class="c-gallery__thumb">
        //    <span class="description">[desc]</span>
        //  </div>
        //</div>

        var arrayImgs = imgs.split(";;");
        var arrayDesc = desc.split(";;");

        var galleryIten = [], index;

        for (index = 0; index < arrayImgs.length; ++index) {
          var f = $("<div>", {class: "iten"}).append(
            $("<img>", {src: arrayImgs[index]}, {alt: arrayDesc[index]}),
            $("<div>", {class: "c-gallery__thumb"}).append(
              $("<span>", {class: "description"}).append(arrayDesc[index])
            )
          );

          galleryIten.push(f);
        }

        $('#modal-gallery .c-gallery .c-gallery__view').slick('unslick').html('').append(galleryIten).slick().resize();

        //Inserir os valores na galeria do modal
        $('#modal-gallery .title').html(title);
        $('#modal-gallery .post-link a').attr("href", href).html(href);
      }

      // Get the value in the href of our button.
      if (check == 'modal-geo' || check == 'modal-loading') {
        $('body').append('<div id="overlay--disable"></div>');
        $('#overlay--disable').fadeIn(200);

        // close alert location
        closeAlertLocation();

        //$('body').append('<div id="overlay" class="geo-confirm-ok"></div>');
        //$('#overlay').fadeIn(200);
      } else {
        $('body').append('<div id="overlay"></div>');
        $('#overlay').fadeIn(200);
      }

      // Fade in the modal window.
      $('#' + check).fadeIn(200);

      //desabilitar o scroll do site
      $('body').addClass('is-no-scrolling');

      //referencia do post no botão de reportar erros/sugestoes
      if( $(this).hasClass('c-report__button') ){
        //insere a referencia no valor do input hidden dentro do modal de reportar
        $('#form-reportar--post').val(ref);
      }

      // BUSCA
      if (check == 'modal-busca') {
        $('#modal-busca').find('.c-modal-search__input').focus();
      }

      // TRACK GA
      trackModal(check, ref, href, title);
    });

    // Loader na busca
    $(document).on('submit', '#modal-busca form', function() {
      $(this).css('display', 'none');
      $('#modal-busca').find('.loader').css('display', 'block');
    });

    //Limpa o player do video ao recarregar a página
    $('#modal-video #player').attr("src", "");

    // Close the modal window and overlay when we click the close button or on the overlay
    $(document).on('click','#overlay, .c-modal__close, .c-dialog__close',function(e) {
      e.preventDefault();

      // Modal Comments box
      if($('.c-comments-box').hasClass('c-comments-box--open')){
        $('.c-comments-box').removeClass('c-comments-box--open');
      }

      $('#overlay, .c-modal, .c-dialog').hide();
      $('#overlay').remove();

      $('body').removeClass('is-no-scrolling');

      //Limpa o player do video
      $('#modal-video #player').attr("src", "");

      // Ativa atualização automática da home
      activeRefreshHome();
    });


    //dialogo de geolocalização
    function showDialog()
    {
      //$('body').append('<div id="overlay"></div>');
      $('body').append('<div id="overlay--disable"></div>');
      $('body').addClass('is-no-scrolling');
      //$('#overlay').fadeIn(200);
      $('#overlay--disable').fadeIn(200);
      $('#modal-geo').fadeIn(300);
    }

    var hasLocationCookie = document.cookie.indexOf("LocationCookie") >= 0;

    function showDialogScroll() {
      if (!hasLocationCookie) {
        // On scroll
        $(window).scroll(function() {
          showDialog();
        });
      }
    }

    //showDialogScroll();

    /*
    (function () {
      var geoConfirm = $('body').hasClass('geo-confirm');
      if (geoConfirm) {
        //setTimeout(showDialog, 1000);
        // On scroll
        $(window).scroll(function(){
          showDialog();
        })
      }
    })();
    */    

    //Exibir select de escolha da cidade
    /*$('.btn-select-city').click(function(e) {
      e.preventDefault();
      $('.c-dialog__search').slideToggle(100);
    });*/

    $(document).on('click','.c-dialog__close',function(e) {
      e.preventDefault();

      $('#overlay--disable, .c-dialog').hide();
      $('#overlay--disable').remove();

      $('body').removeClass('is-no-scrolling');

      //$('.c-dialog__search').hide();
    });


    // MODAL LOADING
    // Open
    $(document).on('submit', '.trigger-form-loading', function() {
      openLoading();
    });

    $(document).on('click', '.trigger-open-loading', function() {
      openLoading();
    });

    // Close
    $(document).on('submit', '.trigger-form-close-loading', function() {
      closeLoading();
    });

    $(document).on('click', '.trigger-close-loading', function() {
      closeLoading();
    });

    // Track GA - Modal Config
    function trackModal (check, ref, href, title) {
      if(typeof dataLayer !== 'undefined') {
        if(typeof ref !== "undefined"){
          ref = ref.replace("https://www.youtube.com/embed/", "");
        }
        if (check == 'modal-video' || check == 'modal-gallery') {
          dataLayer.push({
            'event'              : 'open-modal-interactive',
            'nomeModal'          : check,
            'urlVideoModal'      : ref,
            'urlNoticiaModal'    : href,
            'tituloNoticiaModal' : title,
          });
        } else {
          dataLayer.push({
            'event'     : 'open-modal',
            'nomeModal' : check,
          });
        }
      }
    }


    /*
    -------------------------
    REPORTAR ERROS
    -------------------------
    */
    $(document).on('click', '.btn-reportar', function () {
      var ok = true;

      var inputnome = $("input[name='form-reportar--nome']");
      var inputemail = $("input[name='form-reportar--email']");
      var inputmensagem = $("textarea[name='form-reportar--mensagem']");

      inputnome.next().hide();
      inputemail.next().hide();
      inputemail.next().next().hide();
      inputmensagem.next().hide();
      $(".send .is-error").removeClass("is-show");
      $(".send .is-success").removeClass("is-show");

      if ((/^\s*$/).test(inputnome.val())) {
        ok = false;
        inputnome.next().show();
      }

      if ((/^\s*$/).test(inputemail.val())) {
        ok = false;
        inputemail.next().show();
      } else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) {
        ok = false;
        inputemail.next().next().show();
      }

      if ((/^\s*$/).test(inputmensagem.val())) {
        ok = false;
        inputmensagem.next().show();
      }

      if (ok) {
        $.ajax({
          type: "POST",
          url: $(this).data("urlreport"),
          data: { nome: inputnome.val(), email: inputemail.val(), mensagem: inputmensagem.val(), url: window.location.href },
          success: function() {
            $(".is-success").addClass("is-show").delay(5000).fadeOut("slow");
            inputnome.val('');
            inputemail.val('');
            inputmensagem.val('');
          },
          error: function () {
            $(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
          }
        });
      }
    });

    /*
    -------------------------
    COMPARTILHAR POR EMAIL
    -------------------------
    */
    $(document).on('click', '.btn-share-email', function ()
    {
      var ok = true;

      var inputnome = $("input[name='form-share-email--nome']");
      var inputemail = $("input[name='form-share-email--email']");
      var inputdestinatario = $("input[name='form-share-email-destinatario']");
      var inputmensagem = $("textarea[name='form-share-email--mensagem']");

      inputnome.next().hide();

      inputemail.next().hide();
      inputemail.next().next().hide();

      inputmensagem.next().hide();

      inputdestinatario.next().hide();
      inputdestinatario.next().next().hide();

      $(".send .is-error").removeClass("is-show");
      $(".send .is-success").removeClass("is-show");

      if ((/^\s*$/).test(inputnome.val())) {
        ok = false;
        inputnome.next().show();
      }

      if ((/^\s*$/).test(inputemail.val())) 
      {
        ok = false;
        inputemail.next().show();
      } 
      else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) 
      {
        ok = false;
        inputemail.next().next().show();
      }

      if ((/^\s*$/).test(inputdestinatario.val())) 
      {
        ok = false;
        inputdestinatario.next().show();
      } 
      else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputdestinatario.val())) 
      {
        ok = false;
        inputdestinatario.next().next().show();
      }

      if ((/^\s*$/).test(inputmensagem.val())) {
        ok = false;
        inputmensagem.next().show();
      }

      //console.log(ok)

      if (ok) {
        $.ajax({
          type: "POST",
          url: '/Noticia/ShareEmail',
          data: { nome: inputnome.val(), email: inputemail.val(), emailDest: inputdestinatario.val(), mensagem: inputmensagem.val(), url: window.location.href },
          success: function ()
          {
            $(".is-success").addClass("is-show").delay(5000).fadeOut("slow");
            inputnome.val('');
            inputemail.val('');
            inputdestinatario.val('');
            inputmensagem.val('');
          },
          error: function ()
          {
            $(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
          }
        });
      }
    });


    // GEOLOCATION BROWSER
    if (!hasLocationCookie)
    {
      navigator.geolocation.getCurrentPosition(success, error);

      function success(position) {

        var GEOCODING = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + position.coords.latitude + '%2C' + position.coords.longitude + '&language=en';

        $.getJSON(GEOCODING).done(function (location) {
          var listJson = location.results[0].address_components;

          var cityName;
          //var stateName;
          //var countryName;

          for (var i = 0; i < listJson.length; i++)
          {
            if (location.results[0].address_components[i].types[0] == 'administrative_area_level_2')
              cityName = location.results[0].address_components[i].long_name;

            //if (location.results[0].address_components[i].types[0] == 'administrative_area_level_1')
            //{
            //  stateName = location.results[0].address_components[i].long_name;
            //}

            //if (location.results[0].address_components[i].types[0] == 'country')
            //{
            //  countryName = location.results[0].address_components[i].long_name;
            //}
          }

          //alert('cityName: ' + cityName + ', stateName: ' + stateName + ', countryName: ' + countryName);

          //var idCity;

          //var city = location.results[0].address_components[3].long_name;
          //var state = location.results[0].address_components[5].long_name;

          //$('#country').html(countryName);
          //$('#state').html(stateName);
          //$('#city').html(cityName);
          //$('#address').html(location.results[0].formatted_address);
          //$('#latitude').html(position.coords.latitude);
          //$('#longitude').html(position.coords.longitude);

          // console.log(city);
          // console.log(state);

          //if(state != "Paraná"){
          //  idCity = 12;
          //} else {
          //  if(city == "Curitiba"){
          //    idCity = 12;
          //  }
          //}

          $.get("/getcityidbyname", { cityName: cityName }, function (data){
            //changeLocation(data);
            console.log('1');

            $.post("/changecidade", { id: data }, function (data) {
              console.log('2');
              trackLocation();
              location.reload();
              //closeGeo();
            });
              console.log('2.2');
          });
        })
      }

      function error(err)
      {
        //changeLocation(12);

        $.post("/changecidade", { id: 12 }, function (data) {
          console.log('3');
          trackLocation();
          location.reload();
          //closeGeo();
        });
        
        console.log('4');

        console.log(err);
      }

      function reload()
      {
        openLoading();
        location.reload();
      }
    }

    // Close alert
    $(document).on('click', '.trigger-close-alert-location', function (e) {
      var element = $(this).parents('.alert-location-update');
      if(element.hasClass('mobile')){
        // mobile
        element.html('<span>Você poderá alterar sua localização a qualquer momento clicando no menu.</span>');
        setTimeout(function() {
          closeAlertLocation();
        }, 3500);
      } else {
        // desktop
        closeAlertLocation();
      }
      e.preventDefault();
    });

    var locationAlertClosed = document.cookie.indexOf("location-alert-closed") >= 0;
    if (!locationAlertClosed) {
      $('.alert-location-update').removeClass('close');
    }

    // Close alert + Open modal Geolocation
    $(document).on('click', '.trigger-open-alert-location', function (e) {
      closeAlertLocation();
      showDialog();
      e.preventDefault();
    });

  });
})();

// close alert location
function closeAlertLocation() {
  var myDate = new Date();
  myDate.setFullYear(myDate.getFullYear() + 1);
  document.cookie = 'location-alert-closed=1;expires=' + myDate.toGMTString() + ';path=/';
  $('.alert-location-update').fadeOut();
}

// MODAL LOADING
// Open 
function openLoading() {
  $('body').append('<div id="overlay--disable"></div>');
  $('body').addClass('is-no-scrolling');
  $('#overlay--disable').fadeIn(200);
  $('#modal-loading').fadeIn(300);
}

// Close
function closeLoading() {
  $('#overlay--disable').remove();
  $('body').removeClass('is-no-scrolling');
  $('#overlay--disable').fadeOut(200);
  $('#modal-loading').fadeOut(300);
}

// MODAL GEO
// Close
function closeGeo() {
  $('#overlay--disable').remove();
  $('body').removeClass('is-no-scrolling');
  $('#overlay--disable').fadeOut(200);
  $('#modal-geo').fadeOut(300);
}