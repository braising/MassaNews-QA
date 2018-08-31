/* global $:true */

(function (){
  'use strict';

  // Document.ready
  window.addEventListener('DOMContentLoaded', function (){
    // voltar / goBack
    $(document).on('click', '.trigger-go-back', function (event){
      if ($(this).attr('href') == "#") {
        goBack();
      }
    });

    function goBack() {
      window.history.back();
    }
  });
})();


// VÍDEO INTERNA
var player;
var lastRequestDate = "";

function LoadNexNavegationLink(){
  var lastDate = $(".li-lastnew").last().attr('data-published');

  //console.log(lastDate);

  if (lastRequestDate != lastDate){
    lastRequestDate = lastDate;

    var firstid = $(".li-lastnew").first().attr("id");
    var sectionUrl = $("#SectionUrl").val();

    var url = "/Videos/LoadNexNavegationLink";
    $("<div>").load(url, { firstId: firstid, sectionUrl: sectionUrl, publishDate: lastDate }, function () {
      $(".c-fixed-sidebar__list").append($(this));
    });
  }

  return false;
}

function onYouTubePlayerAPIReady(){
  player = new YT.Player('player',{
    events: {
      'onReady': onPlayerReady,
      'onStateChange': onPlayerStateChange
    },
    playerVars:{
      'showinfo': 0,
      'autoplay': 1,
      'rel': 0
    }
  });
}

function onPlayerReady(){
  var VideoUrl = document.getElementById("VideoUrl").value;
  player.loadVideoByUrl(VideoUrl);
}

function onPlayerStateChange(event){
  var acaoVideo;
  var currentTime = player.getCurrentTime().toString().charAt(0);

  if (event.data === -1){
    acaoVideo = "não iniciado";
  } else if (event.data === 0){
    acaoVideo = "encerrado";
    SetVideo($('.li-lastnew.is-current').next());
  } else if (event.data === 1){
    acaoVideo = "reproduzindo";
  } else if (event.data === 2){
    acaoVideo = "pausado";
  } else if (event.data === 3){
    acaoVideo = "buffer";
  } else if (event.data === 5){
    acaoVideo = "vídeo pronto";
  }
}

function ChangePageState(title, url) {
  if (typeof (history.pushState) != "undefined") {
    var stateData;
    {
      Title = title;
      Url = url;
    }

    //Atualiza o state atual com a nova url
    history.replaceState(stateData, title, url);

    //Altera o título da página
    document.title = title;

    //Faz um push no google analytics para contabilizar o acesso
    if (typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'event': 'virtual-pageview-' + currentCityMicroregiaoUrl,
        'virtualPageURL': url,
        'virtualPageTitle': title
      });
    }
  }
}

function SetVideo(nextli){
  var currentLi = $('.li-lastnew.is-current')[0];
  var urlVideo = $(nextli).attr('data-video');
  var chamadaVideo = $(nextli).attr('data-chamada');

  // Altera o título do vídeo no mobile
  $('.trigger-get-title-video').text(chamadaVideo);

  // Go top
  $("html, body").animate({ scrollTop: 0 }, "slow");

  $(currentLi).removeClass('is-current');
  $(nextli).addClass('is-current');

  player.loadVideoByUrl(urlVideo);

  $(".nano-content").animate({ 'scrollTop': $(".nano-content")[0].scrollTop + $(nextli).position().top }, "slow");

  var newTitle = $(nextli).find('.title a').attr('title');
  var newUrl = $(nextli).find('.title a').attr('href');

  $(".lnk-facebook").attr("href", $(nextli).attr('data-facebook'));
  $(".lnk-twitter").attr("href", $(nextli).attr('data-twitter'));
  $(".lnk-whatsapp").attr("href", $(nextli).attr('data-whatsapp'));
  $(".lnk-gplus").attr("href", $(nextli).attr('data-gplus'));
  $(".lnk-noticia").attr("href", $(nextli).attr('data-noticia'));

  ChangePageState(newTitle, newUrl);
}

$(document).ready(function (){
  $(".nano-content").scroll(function (){
    var trueDivHeight = $('.nano-content')[0].scrollHeight;
    var divHeight = $('.nano-content').height();
    var scrollLeft = trueDivHeight - divHeight;
    var percent = ($(this).scrollTop() * 100) / scrollLeft;

    if (percent >= 50){
      LoadNexNavegationLink();
    }
  });

  $(document).on('click', '.li-lastnew a', function (){
    SetVideo($(this).parents('li')[0]);
    return false;
  });
});