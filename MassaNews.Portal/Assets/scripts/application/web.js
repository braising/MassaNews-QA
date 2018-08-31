$(function () {

  "use strict";

  /*
    -----------------------------------------
    CONFIRMAR/ALTERAR CIDADE
    -----------------------------------------
  */
  $('.geo-confirm-ok').on('click', function () {
    // Sem cookie
    if (document.cookie.indexOf("LocationCookie") < 0) {

      loaderGeo();

      $.post("/changecidade", { id: $(".select-cidades").val() }, function (data) {
        trackLocation();
        location.reload();
        //closeGeo();
      });

      /* capa única (Curitiba) */
      //changeLocation($(".select-cidades").val());
      //location.reload();
    }
  });

  // Fora do Parana (id: 12/Curitiba)
  $('.geo-confirm-parana').on('click', function () {

    var cookieValue = getCookie("LocationCookie");

    if(cookieValue == 12){
      //Close Geo Location Modal
      closeGeo();
    } else {
      loaderGeo();

      $.post("/changecidade", { id: 12 }, function (data) {
        trackLocation();
        location.reload();
        //closeGeo();
      });
    }

    /* capa única (Curitiba) */
    //changeLocation(12);
    //location.reload();
  });

  // Select de cidades
  $(".select-cidades").on("change", function () {

    loaderGeo();

    if (document.cookie.indexOf("LocationCookie") < 0) {
      // Sem cookie
      $.post("/changecidade", { id: $(this).val() }, function (data) {
        trackLocation();
        location.reload();

        //closeGeo();
      });
    } else {
      // Com cookie
      $.post("/changecidade", { id: $(this).val() }, function (data){
        trackLocation();
        window.location = home;

        //closeGeo();
      });
    }

    /* capa única (Curitiba) */
    //changeLocation($(this).val());
    //location.reload();
  });

  // Atualiza o cookie de localizacao
  $('.trigger-geo-update').on('click', function (event){
    var $geoId = $(this).data('geo-id');

    $.post("/changecidade", { id: $geoId }, function (data) {
      trackLocation();
      window.location = home;
      //closeGeo();
    });
    
    //changeLocation($geoId);
    //location.reload();
    event.preventDefault();
  });

  /*
    -------------------------
    BUSCA - ULTIMAS PALAVRAS UTILIZADAS
    -------------------------
  */
  $('.lastwords').on('click', function () {
    var input = $(this).parent().parent().parent().children("div.c-search__input-box").children("input.c-search__input");
    if (input.val() !== "")
      input.val(input.val() + ' ' + $(this).text());
    else
      input.val($(this).text());
  });


  /*
    -------------------------
    BUSCA - VALIDACAO
    -------------------------
  */
  $(".c-search__submit").on("click", function (event) {
    if ($(this).prev().find('.c-search__input').val() === "" || $(this).prev().find('.c-search__input').val().length < 3)
      return false;

    var form = $("form.form-search-search");
    form.submit();
  });


  /*
    -------------------------
    NEWSLETTER
    -------------------------
  */
  $(".btn-newsletter").on("click", function () {
    var ok = true;
    var inputnome = $("input[name='nome']");
    var inputemail = $("input[name='form-nl-email']");
    var status;

    inputnome.next().hide();
    inputemail.next().hide();
    inputemail.next().next().hide();

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

    if (ok) {
      status = 'ok';

      $.ajax({
        type: "POST",
        url: $(this).data("urlsubscribe"),
        data: { nome: inputnome.val(), email: inputemail.val() }
      }).done(function () {
        $(".c-newsletter__feedback").show().delay(5000).fadeOut("slow");
        inputnome.val('');
        inputemail.val('');
      });
    } else {
      status = 'error';
    }

    // Track GA
    if(typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'event' : 'newsletter',
        'status-newsletter' : status,
      });
    }
  });


  /*
    -------------------------
    NEWSLETTER UPDATE
    -------------------------
  */
  $(".trigger-newsletter-update").on("click", function ()
  {
    var ok = true;

    var inputhash = $("input[name='form-newsletter-update--hash']");
    var inputnome = $("input[name='form-newsletter-update--nome']");
    var inputemail = $("input[name='form-newsletter-update--email']");
    var inputcidade = $("input[name='form-newsletter-update--cidade']");
    var inputcell = $("input[name='form-newsletter-update--celular']");
    var city = $("#CityLetter");
    var period = $("input:checked[type=radio]");
    var preferences = [];

    $("input:checked[type=checkbox]").each(function (index){
      preferences.push($(this).val());
    });

    var status;

    inputnome.next().hide();
    inputcidade.next().hide();
    inputemail.next().hide();
    //inputemail.next().next().hide();

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

    if ((/^\s*$/).test(inputcidade.val()))
    {
      ok = false;
      inputcidade.next().show();
    }

    if (ok){
      status = 'ok';

      var data ={
        Hash : inputhash.val(),
        Name: inputnome.val(),
        Email: inputemail.val(),
        CellPhone: inputcell.val(),
        Period: period.val(),
        City: city.val(),
        Preferences: preferences
      };

      $.ajax({
        type: "POST",
        url: $(this).data("urlsubscribe"),
        data: data,
        success: function() {
          $(".is-success").addClass("is-show").delay(5000).fadeOut("slow");
          $('body,html').animate({scrollTop: 0});
        },
        error: function () {
          $(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
          $('body,html').animate({scrollTop: 0});
        }
      });
    } else { status = 'error'; }

    // Track GA
    if(typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'event' : 'newsletter-update',
        'status-newsletter' : status,
      });
    }
  });


  /*
    -------------------------
    NEWSLETTER UNSUBSCRIBE
    -------------------------
  */
  $(document).on('click', '.trigger-newsletter-unsubscribe', function () {

    var ok = true;
    var radios = $('input[name=unsubscribe]:checked').attr('value');
    var inputhash = $("input[name='form-newsletter-update--hash']");

    $('.field-validation-error').hide();
    $(".send .is-error").removeClass("is-show");
    $(".send .is-success").removeClass("is-show");

    if (radios == undefined) {
      ok = false;
      $('.field-validation-error').show();
    }

    if (ok)
    {
      var data = 
      { 
        Hash: inputhash.val(),
        Reason: radios
      };

      $.ajax({
        type: "POST",
        url: $(this).data("urlunsubscribe"),
        data: data,
        success: function() 
        {
          $(".unsubscribe").hide();
          $(".no-unsubscribe").removeClass('u-none').fadeIn();
          $(".is-success").addClass("is-show");
          $('body,html').animate({scrollTop: 0});
        },
        error: function () 
        {
           status = 'error';
           $(".is-error").addClass("is-show").delay(5000).fadeOut();
           $('body,html').animate({scrollTop: 0});
        }
      });
    } 
    else { status = 'error'; }

    // Track GA
    if(typeof dataLayer != 'undefined') {
      dataLayer.push({
        'event' : 'newsletter-no-unsubscribe',
        'status-newsletter' : status,
      });
    }
  });


  /*
    -------------------------
    LAND BCK - PLANEJAMENTO DE VIDA
    -------------------------
  */
  $(document).on('click', '.trigger-land-bck', function (e) {

    e.preventDefault();

    var ok = true;

    var inputnome = $("input[name='form-bck-nome']");
    var inputtelefone = $("input[name='form-bkc--telefone']");
    var inputemail = $("input[name='form-bkc--email']");

    inputnome.next().hide();
    inputtelefone.next().hide();
    inputemail.next().hide();
    inputemail.next().next().hide();

    if ((/^\s*$/).test(inputnome.val())) {
      ok = false;
      inputnome.next().show();
    }

    if ((/^\s*$/).test(inputtelefone.val())) {
      ok = false;
      inputtelefone.next().show();
    }

    if ((/^\s*$/).test(inputemail.val())) {
      ok = false;
      inputemail.next().show();
    } else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) {
      ok = false;
      inputemail.next().next().show();
    }

    if (ok){
      // Loading
      openLoading();

      $.ajax({
        type: "POST",
        url: $(this).data("/planejamento-de-vida"),
        data:
        {
          Nome: inputnome.val(),
          Email: inputemail.val(),
          Telefone: inputtelefone.val(),
        },
        success: function () {
          closeLoading();
          //alert('Dados enviados com sucesso!');
          //$("html, body").animate({ scrollTop: 0 });

          inputnome.val("");
          inputtelefone.val("");
          inputemail.val("");

          $('.is-success').removeClass('u-none').delay(5000).fadeOut("slow");
        },
        error: function (e) {
          closeLoading();
          //alert('Ops! Algo deu errado, tente novamente.');
          $(".is-error").removeClass("u-none").delay(5000).fadeOut("slow");
        }
      });
    }
  });

  /*
    -------------------------
    WHERE 15 ANOS
    -------------------------
  */
  // $(document).on('click', '.trigger-inscricao-where-15-anos', function() {
  //   var ok = true;

  //   var inputnome = $("input[name='form-inscricao--nome']");
  //   var inputemail = $("input[name='form-inscricao--email']");
  //   var inputtelefone = $("input[name='form-inscricao--telefone']");
  //   var inputnascimento = $("input[name='form-inscricao--nascimento']");
  //   var inputcidade = $("input[name='form-inscricao--cidade']");
  //   var inputtitulo = $("input[name='form-inscricao--titulo']");
  //   var inputimagem = document.getElementById("form-inscricao--imagem");
  //   var inputimagemerror = $('.invalid-imagem');
  //   var inputregulamento = $('.invalid-regulamento');
  //   var inputregulamentocheck = $('#form-inscricao--regulamento:checkbox:checked').length > 0;
  //   var inputnewscheck = $('#form-inscricao--news:checkbox:checked').length > 0;

  //   inputnome.next().hide();
  //   inputemail.next().hide();
  //   inputemail.next().next().hide();
  //   inputtelefone.next().hide();
  //   inputnascimento.next().hide();
  //   inputcidade.next().hide();
  //   inputtitulo.next().hide();
  //   inputimagemerror.hide();
  //   inputregulamento.hide();

  //   $(".send .is-error").removeClass("is-show");
  //   $(".send .is-success").removeClass("is-show");

  //   // Nome
  //   if ((/^\s*$/).test(inputnome.val())) {
  //     ok = false;
  //     inputnome.next().show();
  //   }

  //   // Email
  //   if ((/^\s*$/).test(inputemail.val())) {
  //     ok = false;
  //     inputemail.next().show();
  //   } else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) {
  //     ok = false;
  //     inputemail.next().next().show();
  //   }

  //   // Telefone
  //   if ((/^\s*$/).test(inputtelefone.val())) {
  //     ok = false;
  //     inputtelefone.next().show();
  //   }

  //   // Data de nascimento
  //   if ((/^\s*$/).test(inputnascimento.val())) {
  //     ok = false;
  //     inputnascimento.next().show();
  //   }

  //   // Cidade
  //   if ((/^\s*$/).test(inputcidade.val())) {
  //     ok = false;
  //     inputcidade.next().show();
  //   }

  //   // Titulo
  //   if ((/^\s*$/).test(inputtitulo.val())) {
  //     ok = false;
  //     inputtitulo.next().show();
  //   }

  //   // Imagem
  //   if (inputimagem.files.length == 0) {
  //     ok = false;
  //     inputimagemerror.html('*Obrigatório');
  //     inputimagemerror.show();
  //   }
  //   else if (inputimagem.files[0].size > 5242880)
  //   {
  //     ok = false;
  //     inputimagemerror.html('*Imagem acima de 5mb');
  //     inputimagemerror.show();
  //   }

  //   // Regulamento
  //   if(!inputregulamentocheck){
  //     ok = false;
  //     inputregulamento.show();
  //   }

  //   if (ok){
  //     var reader = new FileReader();

  //     // Loading
  //     openLoading();

  //     reader.onload = function (e)
  //     {
  //       $.ajax({
  //         type: "POST",
  //         url: $(this).data("/promocao/where-15-anos/enviar"),
  //         data:
  //         {
  //           Nome: inputnome.val(),
  //           Email: inputemail.val(),
  //           Telefone: inputtelefone.val(),
  //           Nascimento: inputnascimento.val(),
  //           Cidade: inputcidade.val(),
  //           Titulo: inputtitulo.val(),
  //           Imagem: e.target.result,
  //           OptInNews: inputnewscheck
  //         },
  //         success: function ()
  //         {
  //           closeLoading();
  //           $("html, body").animate({ scrollTop: 0 });
  //           $('.trigger-form-cadastro').addClass('u-none');
  //           $('.trigger-form-cadastro-sucesso').removeClass('u-none');
  //         },
  //         error: function (e)
  //         {
  //           alert('Ops! Algo deu errado, tente novamente.')
  //           //$(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
  //         }
  //       });
  //     }
  //     reader.readAsDataURL(inputimagem.files[0]);
  //   }
  // });

  // // Btn votado
  // $(document).on('click', '.trigger-btn-votado', function (event){
  //   alert('Você já votou nesta foto :)')
  //   event.preventDefault();
  // });

  // // MOSAIC - WHERE 15 ANOS
  // $(document).on('touchstart click', '.trigger-where-teste', function (event){
  //   //event.preventDefault();
  //   event.stopPropagation();
  //   vote($(this));
  // });

  // // SHARE - WHERE 15 ANOS
  // $(document).on('touchstart click', '.trigger-share-where', function (event){
  //   //event.preventDefault();
  //   event.stopPropagation();

  //   var imgId = $(this).parents('.vote').find('button:first').data('img');

  //   //Set the url
  //   var a = document.createElement('a');
  //   a.href = location.href;
  //   var domainUrl = a.protocol + "//" + (a.hostname || a.pathname) + '/promocao/where-15-anos/' + imgId + '#nanogallery/g-0/0/' + imgId;

  //   var social = $(this).data("social");
  //   var $tile = $(this).parents("#nanoGalleryViewer");
  //   var image = $tile.find('.imgCurrent').attr("src");

  //   if (!image)
  //     image = $tile.find(".item").attr("src");

  //   var text = $.trim($tile.find(".description").text());
  //   var title = $.trim($tile.find(".title").text());

  //   if (!text.length)
  //     text = document.title;

  //   if (social == "facebook"){
  //     var url = "https://www.facebook.com/dialog/feed?app_id=942322269150239&" + "link=" + encodeURIComponent(domainUrl) + "&" + "display=popup&" + "name=" + encodeURIComponent(document.title) + "&" + "caption=massanews.com&" + "description=Foto '" + encodeURIComponent(text) + "' de '" + encodeURIComponent(title) + "'. Vote para escolher os finalistas!&" + "picture=" + qualifyURL(image) + "&" + "ref=share&" + "redirect_uri=" + encodeURIComponent(domainUrl);
  //     var w = window.open(url, "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "twitter"){
  //     var w = window.open("https://twitter.com/intent/tweet?url=" + encodeURI(domainUrl.split('#')[0]) + "&text=" + encodeURI(text) + " - " + encodeURI(title), "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "google-plus"){
  //     var url = "https://plus.google.com/share?url=" + encodeURI(domainUrl);
  //     var w = window.open(url, "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "whatsapp"){
  //     var w = window.open("whatsapp://send?text=" + encodeURI(text) + " " + encodeURI(domainUrl) + "?utm_source=whatsapp&utm_medium=share&utm_content=share-foto&utm_campaign=where-15-anos");
  //     //w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }
  // });




  /*
    -------------------------
    NATAL DE LUZ
    -------------------------
  // */
  // $(document).on('click', '.trigger-inscricao-natal-de-luz', function() {
  //   var ok = true;

  //   var inputnome = $("input[name='form-inscricao--nome']");
  //   var inputemail = $("input[name='form-inscricao--email']");
  //   var inputtelefone = $("input[name='form-inscricao--telefone']");
  //   var inputnascimento = $("input[name='form-inscricao--nascimento']");
  //   var inputcidade = $("input[name='form-inscricao--cidade']");
  //   var inputtitulo = $("input[name='form-inscricao--titulo']");
  //   var inputcpf = $("input[name='form-inscricao--cpf']");
  //   var inputimagemcasa = document.getElementById("form-inscricao--imagem-casa");
  //   var inputimagemrua = document.getElementById("form-inscricao--imagem-rua");
  //   var inputimagemcomprovante = document.getElementById("form-inscricao--imagem-comprovante");
  //   var inputimagemcasaerror = $('.invalid-imagem-casa');
  //   var inputimagemruaerror = $('.invalid-imagem-rua');
  //   var inputimagemcomprovanteerror = $('.invalid-imagem-comprovante');
  //   var inputregulamentoerror = $('.invalid-regulamento');
  //   var inputregulamentocheck = $('#form-inscricao--regulamento:checkbox:checked').length > 0;
  //   var inputtipoerror = $('.invalid-tipo');
  //   var inputtipocheck = $('input[name="form-inscricao--tipo"]:radio:checked').length > 0;
  //   var inputtipocasacheck = $('#form-inscricao--tipo-casa:radio:checked').length > 0;
  //   var inputtiporuacheck = $('#form-inscricao--tipo-rua:radio:checked').length > 0;
  //   var inputnewscheck = $('#form-inscricao--news:checkbox:checked').length > 0;

  //   inputnome.next().hide();
  //   inputemail.next().hide();
  //   inputemail.next().next().hide();
  //   inputtelefone.next().hide();
  //   inputnascimento.next().hide();
  //   inputcidade.next().hide();
  //   inputtitulo.next().hide();
  //   inputcpf.next().hide();
  //   inputcpf.next().next().hide();
  //   inputimagemcasaerror.hide();
  //   inputimagemruaerror.hide();
  //   inputimagemcomprovanteerror.hide();
  //   inputregulamentoerror.hide();
  //   inputtipoerror.hide();

  //   $(".send .is-error").removeClass("is-show");
  //   $(".send .is-success").removeClass("is-show");

  //   // Nome
  //   if ((/^\s*$/).test(inputnome.val())) {
  //     ok = false;
  //     inputnome.next().show();
  //   }

  //   // Email
  //   if ((/^\s*$/).test(inputemail.val())) {
  //     ok = false;
  //     inputemail.next().show();
  //   } else if (!(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/).test(inputemail.val())) {
  //     ok = false;
  //     inputemail.next().next().show();
  //   }

  //   // Telefone
  //   if ((/^\s*$/).test(inputtelefone.val())) {
  //     ok = false;
  //     inputtelefone.next().show();
  //   }

  //   // Data de nascimento
  //   if ((/^\s*$/).test(inputnascimento.val())) {
  //     ok = false;
  //     inputnascimento.next().show();
  //   }

  //   // Cidade
  //   if ((/^\s*$/).test(inputcidade.val())) {
  //     ok = false;
  //     inputcidade.next().show();
  //   }

  //   // Titulo
  //   if ((/^\s*$/).test(inputtitulo.val())) {
  //     ok = false;
  //     inputtitulo.next().show();
  //   }

  //   // CPF
  //   if ((/^\s*$/).test(inputcpf.val())){
  //     ok = false;
  //     inputcpf.next().show();
  //   } else if (!validaCPF(inputcpf.val()) ) {
  //     ok = false;
  //     inputcpf.next().next().show();
  //   }

  //   // Imagem CASA
  //   if(inputtipocasacheck){
  //     if (inputimagemcasa.files.length == 0) {
  //       ok = false;
  //       inputimagemcasaerror.html('*Obrigatório');
  //       inputimagemcasaerror.show();
  //     } else if (inputimagemcasa.files[0].size > 5242880){
  //       ok = false;
  //       inputimagemcasaerror.html('*Imagem acima de 5mb');
  //       inputimagemcasaerror.show();
  //     }
  //   }

  //   // Imagem RUA
  //   if(inputtiporuacheck){
  //     if (inputimagemrua.files.length == 0) {
  //       ok = false;
  //       inputimagemruaerror.html('*Obrigatório');
  //       inputimagemruaerror.show();
  //     } else if (inputimagemrua.files[0].size > 5242880){
  //       ok = false;
  //       inputimagemruaerror.html('*Imagem acima de 5mb');
  //       inputimagemruaerror.show();
  //     }
  //   }

  //   // Imagem COMPROVANTE
  //   if (inputimagemcomprovante.files.length == 0) {
  //     ok = false;
  //     inputimagemcomprovanteerror.html('*Obrigatório');
  //     inputimagemcomprovanteerror.show();
  //   } else if (inputimagemcomprovante.files[0].size > 5242880){
  //     ok = false;
  //     inputimagemcomprovanteerror.html('*Imagem acima de 5mb');
  //     inputimagemcomprovanteerror.show();
  //   }

  //   // Regulamento
  //   if(!inputregulamentocheck){
  //     ok = false;
  //     inputregulamentoerror.show();
  //   }

  //   // Tipo
  //   if(!inputtipocheck){
  //     ok = false;
  //     inputtipoerror.show();
  //   }

  //   //console.log('1');

  //   if (ok) {
  //       openLoading();

  //       var data = new FormData();

  //       data.append('Nome', inputnome.val());
  //       data.append('Email', inputemail.val());
  //       data.append('Telefone', inputtelefone.val());
  //       data.append('Nascimento', inputnascimento.val());
  //       data.append('Cidade', inputcidade.val());
  //       data.append('Titulo', inputtitulo.val());
  //       data.append('CPF', inputcpf.val());        
  //       data.append('OptInNews', inputnewscheck);
  //       data.append('TipoCasa', inputtipocasacheck);
  //       data.append('TipoRua', inputtiporuacheck);

  //       if (inputimagemcasa.files.length > 0) {
  //           data.append('ImagemCasa', inputimagemcasa.files[0]);
  //       }
                
  //       if (inputimagemrua.files.length > 0) {
  //           data.append('ImagemRua', inputimagemrua.files[0]);
  //       }
                
  //       if (inputimagemcomprovante.files.length > 0) {
  //           data.append('ImagemComprovante', inputimagemcomprovante.files[0]);
  //       }                    

  //       $.ajax({
  //         type: "POST",
  //         url: "/natal-de-luz/enviar",
  //         data: data,
  //         contentType: false,
  //         processData: false,
  //         dataType: 'json',
  //         success: function ()
  //         {
  //           closeLoading();
  //           $("html, body").animate({ scrollTop: 0 });
  //           $('.trigger-form-cadastro').addClass('u-none');
  //           $('.trigger-form-cadastro-sucesso').removeClass('u-none');
  //         },
  //         error: function (e)
  //         {
  //           alert('Ops! Algo deu errado, tente novamente.')
  //           //$(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
  //         }
  //       });


  //   //var files = document.getElementById('form-natal-de-luz').files;
  //   //console.log(files);
  //   //  //console.log('2');
  //   //  var reader = new FileReader();

  //   //  // Loading
  //   //  openLoading();

  //   //  reader.onload = function (e)
  //   //  {
  //   //    $.ajax({
  //   //      type: "POST",
  //   //      url: $(this).data("/natal-de-luz/enviar"),
  //   //      data:
  //   //      {
  //   //        Nome: inputnome.val(),
  //   //        Email: inputemail.val(),
  //   //        Telefone: inputtelefone.val(),
  //   //        Nascimento: inputnascimento.val(),
  //   //        Cidade: inputcidade.val(),
  //   //        Titulo: inputtitulo.val(),
  //   //        CPF: inputcpf.val(),
  //   //        ImagemCasa: e.target.result,
  //   //        OptInNews: inputnewscheck, 
  //   //        TipoCasa: inputtipocasacheck,
  //   //        TipoRua: inputtiporuacheck
  //   //      },
  //   //      success: function ()
  //   //      {
  //   //        closeLoading();
  //   //        $("html, body").animate({ scrollTop: 0 });
  //   //        $('.trigger-form-cadastro').addClass('u-none');
  //   //        $('.trigger-form-cadastro-sucesso').removeClass('u-none');
  //   //      },
  //   //      error: function (e)
  //   //      {
  //   //        alert('Ops! Algo deu errado, tente novamente.')
  //   //        //$(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
  //   //      }
  //   //    });
  //   //  }
  //   //  reader.readAsDataURL(inputimagemcasa.files[0]);
  //   //  reader.readAsDataURL(inputimagemrua.files[0]);
  //   //  reader.readAsDataURL(inputimagemcomprovante.files[0]);

  //       ////console.log(files);
  //       //var files = new FormData(document.querySelector('form-natal-de-luz'));
  //       //console.log(files);

  //       //  $.ajax({
  //       //  type: "POST",
  //       //  url: $(this).data("/natal-de-luz/enviar"),
  //       //  data:files,
  //       //  success: function () {
  //       //      closeLoading();
  //       //      $("html, body").animate({ scrollTop: 0 });
  //       //      $('.trigger-form-cadastro').addClass('u-none');
  //       //      $('.trigger-form-cadastro-sucesso').removeClass('u-none');
  //       //  },
  //       //  error: function (e) {
  //       //      //e.Error
  //       //      alert('Ops! Algo deu errado, tente novamente.')
  //       //      //$(".is-error").addClass("is-show").delay(5000).fadeOut("slow");
  //       //  }
  //     //});
  //   }
  // });

  // // Tipo
  // $("#form-inscricao--tipo-casa:radio").change(function () {
  //   $('.input-imagem-rua').hide();
  //   $('.input-imagem-casa, .input-imagem-comprovante').show();
  // });

  // $("#form-inscricao--tipo-rua:radio").change(function () {
  //   $('.input-imagem-casa').hide();
  //   $('.input-imagem-rua, .input-imagem-comprovante').show();
  // });

  // // Btn votado
  // $(document).on('click', '.trigger-btn-votado', function (event){
  //   alert('Você já votou nesta foto :)')
  //   event.preventDefault();
  // });

  // // MOSAIC - NATAL DE LUZ
  // $(document).on('touchstart click', '.trigger-where-teste', function (event){
  //   //event.preventDefault();
  //   event.stopPropagation();
  //   vote($(this));
  // });

  // // SHARE - NATAL DE LUZ
  // $(document).on('touchstart click', '.trigger-share-natal-de-luz', function (event){
  //   //event.preventDefault();
  //   event.stopPropagation();

  //   var imgId = $(this).parents('.vote').find('button:first').data('img');

  //   //Set the url
  //   var a = document.createElement('a');
  //   a.href = location.href;
  //   var domainUrl = a.protocol + "//" + (a.hostname || a.pathname) + '/natal-de-luz/' + imgId + '#nanogallery/g-0/0/' + imgId;

  //   var social = $(this).data("social");
  //   var $tile = $(this).parents("#nanoGalleryViewer");
  //   var image = $tile.find('.imgCurrent').attr("src");

  //   if (!image)
  //     image = $tile.find(".item").attr("src");

  //   var text = $.trim($tile.find(".description").text());
  //   var title = $.trim($tile.find(".title").text());

  //   if (!text.length)
  //     text = document.title;

  //   if (social == "facebook"){
  //     var url = "https://www.facebook.com/dialog/feed?app_id=164634034123946&" + "link=" + encodeURIComponent(domainUrl) + "&" + "display=popup&" + "name=" + encodeURIComponent(document.title) + "&" + "caption=massanews.com&" + "description=Foto '" + encodeURIComponent(text) + "' de '" + encodeURIComponent(title) + "'. Vote para escolher os finalistas!&" + "picture=" + qualifyURL(image) + "&" + "ref=share&" + "redirect_uri=" + encodeURIComponent(domainUrl);
  //     var w = window.open(url, "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "twitter"){
  //     var w = window.open("https://twitter.com/intent/tweet?url=" + encodeURI(domainUrl.split('#')[0]) + "&text=" + encodeURI(text) + " - " + encodeURI(title), "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "google-plus"){
  //     var url = "https://plus.google.com/share?url=" + encodeURI(domainUrl);
  //     var w = window.open(url, "foo", "");
  //     w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }

  //   if (social == "whatsapp"){
  //     var w = window.open("whatsapp://send?text=" + encodeURI(text) + " " + encodeURI(domainUrl) + "?utm_source=whatsapp&utm_medium=share&utm_content=share-foto&utm_campaign=natal-de-luz");
  //     //w.moveTo((screen.width / 2) - (300), (screen.height / 2) - (200));
  //   }
  // });




  var qualifyURL = function(url) {
    var img = document.createElement('img');
    img.src = url;
    url = img.src;
    img.src = null;
    return url;
  };

});


// NOTIFICATION
// Determine the correct object to use
var notification = window.Notification || window.mozNotification || window.webkitNotification;

// The user needs to allow this
if ('undefined' != typeof notification){
  document.addEventListener('DOMContentLoaded', function () {
    if (Notification.permission !== "granted")
      Notification.requestPermission();
  });
}

window.setInterval(function () {
  if ('undefined' != typeof notification) {
    //if (Notification.permission !== "granted")
    //  Notification.requestPermission();
    if (Notification.permission == "granted") {
      var idLastestNoticia = document.cookie.replace(/(?:(?:^|.*;\s*)IdHighlight\s*\=\s*([^;]*).*$)|^.*$/, "$1");
      if (idLastestNoticia == '' || idLastestNoticia == 'undefined') idLastestNoticia = null;

      $.get('https://massanews.com/Home/GetNotification', { idNoticia: idLastestNoticia }, function (data) {

        if (data.ShowNoticia) {
          var notification = new Notification(data.Title, {
            icon: data.IconUrl,
            IconUrl: data.IconUrl,
            tag: 'massanews-notify',
            body: data.Body
          });
          notification.onclick = function () {
            window.open(data.NoticiaURL);
          };
          //fecha a janela de notificação por um tempo determinado em milisegundos
          setTimeout(notification.close.bind(notification), 10000);
        }
        var myDate = new Date();
        myDate.setFullYear(myDate.getFullYear() + 1);

        document.cookie = 'IdHighlight=' + data.Id + ';expires=' + myDate.toGMTString() + ';path=/';
      });
    }
  }
}, 60000);

/*
 * FUNCTIONS
 */

// Loader Geo
function loaderGeo () {
  $('#modal-geo').find('.c-button--small').addClass('loader');
}

// Track Location - GA
//function trackLocation (oldMicroregionSlug, newMicroregionSlug) {
function trackLocation () {
  if(typeof dataLayer !== 'undefined') {

    dataLayer.push({
      'track-action': $(".select-cidades option:selected").text(),
      'event'       : 'location-updated'
    });

    //Faz um push no google analytics para contabilizar o pageview
    // var url   = window.location.pathname;
    // var title = $('title').text();
    // var isHome = $('body').hasClass('pg-home');
    // if (isHome){
    //   dataLayer.push({
    //     'event': 'virtual-pageview-' + newMicroregionSlug,
    //     'virtualPageURL': url,
    //     'virtualPageTitle': title
    //   });
    // }
  }
}


// Change Location 
// function changeLocation(locationId) {

//   debugger;
//   var currentLocationId = getCookie("LocationCookie");

//   if (currentLocationId == null || currentLocationId == ''){
//     currentLocationId = 12;
//   }

//   $.post("/changecidade", { oldId: currentLocationId, newId: locationId }, function (data)
//   {
//     var oldMicroregionId = data.split(',')[0];
//     var newMicroregionId = data.split(',')[1];
//     var oldMicroregionSlug = data.split(',')[2];
//     var newMicroregionSlug = data.split(',')[3];

//     // Atualiza a variável da microrregião ativa
//     currentCityMicroregiaoUrl = newMicroregionSlug;

//     //Track the Location 
//     trackLocation(oldMicroregionSlug, newMicroregionSlug);

//     //Close Geo Location Modal
//     closeGeo();
    
//     //Change Weather
//     if (currentLocationId != null && currentLocationId != locationId) {
//       $.get('/home/GetWeatherByLocationId', { locationId: locationId }, function (data) {
//         $(".trigger-get-city-location").html($(data)[0].Cidade);
//         $('.max .temp').html($(data)[0].simeparPrevisoes[0].tempMax);
//         $('.min .temp').html($(data)[0].simeparPrevisoes[0].tempMin);
//         $('.sky .wi').removeClass().addClass('wi').addClass($(data)[0].simeparPrevisoes[0].simeparPeriodos[0].icon);
//       });

//       window.location.reload(true);
//       //window.location.href = "/{Home}/{Index}";

//       // Replace "Sua Região" - Home
//       //var isHome = $('body').hasClass('pg-home');

//       //if (isHome)
//       //{
//       //  //Change with data of the other microregion
//       //  if (oldMicroregionId != newMicroregionId)
//       //  {
//       //    //EDITORIAIS
//       //    var lstModules =
//       //    [
//       //      { id: 1, url: 'plantao' },
//       //      { id: 2, url: 'esportes' },
//       //      //{ id: 3, url: 'parana' },
//       //      //{ id: 9, url: 'entretenimento' }
//       //    ];

//       //    $(lstModules).each(function (index)
//       //    {
//       //      var id = $(this)[0].id;
//       //      var url = $(this)[0].url;

//       //      // Add Loader
//       //      $('#section-' + url + ' .loader-section').removeClass('u-none');
//       //      $('#section-' + url + ' .link-see-all').removeClass('fa-plus-circle').addClass('loader');

//       //      $.get('/home/loadsectionmodule', { locationId: locationId, sectionId: id }, function (data)
//       //      {
//       //        //Replace the content
//       //        $('.pg-home #section-' + url).replaceWith(data);
//       //        // Remove Loader
//       //        $('#section-' + url + ' .loader-section').addClass('u-none');
//       //        $('#section-' + url + ' link-see-all').removeClass('loader').addClass('fa-plus-circle');
//       //      });

//       //    });

//       //    $.get('/home/loadsectionmodule', { locationId: locationId, sectionId: 11 }, function (data){
//       //      $("#section-videos").replaceWith(data);
//       //      //Set Slider
//       //      sliderVideos();
//       //    });

//       //    //WHERE CURITIBA
//       //    if (newMicroregionId == 1)
//       //      $.get('/home/loadsectionmodule', { locationId: locationId, sectionId: 13 }, function (data) {
//       //        $(".pg-home .c-gallery").after(data);
//       //      });
//       //    else
//       //      $("#section-where-curitiba").remove();

//       //    //BLOGS
//       //    $.get('/home/loadblogsmodule', { microregionId: newMicroregionId }, function (data)
//       //    {
//       //      $(".sidebar__blogs").replaceWith(data);
//       //    });
//       //  }
//       //}
//     }
//   });
// }

// Get Cookie Value
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

function setCookie(cname, cvalue, hours) {
  var d = new Date();
  d.setTime(d.getTime() + (hours * 60 * 60 * 1000));
  var expires = "expires=" + d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}


// VALIDAÇÃO DE CPF
function CPFIsValid(strCPF) {
  strCPF = strCPF.replace('.', '').replace('.', '').replace('-', '');

  var Soma;
  var Resto;

  Soma = 0;
  if (strCPF == "00000000000") return false;

  for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
  Resto = (Soma * 10) % 11;

  if ((Resto == 10) || (Resto == 11)) Resto = 0;
  if (Resto != parseInt(strCPF.substring(9, 10))) return false;

  Soma = 0;
  for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
  Resto = (Soma * 10) % 11;

  if ((Resto == 10) || (Resto == 11)) Resto = 0;
  if (Resto != parseInt(strCPF.substring(10, 11))) return false;
  return true;
}

function validaCPF(cpf){
  var numeros, digitos, soma, i, resultado, digitos_iguais;
  digitos_iguais = 1;
  if (cpf.length < 11)
    return false;

  for (i = 0; i < cpf.length - 1; i++)
  if (cpf.charAt(i) != cpf.charAt(i + 1)){
    digitos_iguais = 0;
    break;
  }
  if (!digitos_iguais)
    {
    numeros = cpf.substring(0,9);
    digitos = cpf.substring(9);
    soma = 0;
    for (i = 10; i > 1; i--)
      soma += numeros.charAt(10 - i) * i;
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
      return false;
    numeros = cpf.substring(0,10);
    soma = 0;
    for (i = 11; i > 1; i--)
      soma += numeros.charAt(11 - i) * i;
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
      return false;
    return true;
    }
  else
    return false;
}