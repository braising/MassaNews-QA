(function (window, $) {

  'use strict';

  $(document).ready(function(){

    //slider vídeos home
    sliderVideos();

    //slider destaques (roteiros)
    sliderHeadlinesBg();

    //slider tabela campeonato
    sliderTable();

    sliderBolao();

    // CONFIGURA DATES
    // currentRoundA = {
    //   "name":"Brasileirão Série A",
    //   "dates": [
    //     {
    //       "year": 2018,
    //       "month": 4,
    //       "days":[ 14, 21, 28 ]
    //     }, {
    //       "year": 2018,
    //       "month": 5,
    //       "days":[ 5, 12, 19, 26 ]
    //     },
    //   ]
    // }

    // var tdate = new Date();
    // //var dd = tdate.getDate(); // day
    // //var MM = tdate.getMonth() + 1; // month
    
    // var dd = 28; // day
    // var MM = 5; // month

    // //var yyyy = tdate.getFullYear(); // year
    // //var currentDate= dd + "-" +( MM+1) + "-" + yyyy;

    // if(MM == 4){
    //   if(dd <= 20){
    //     var currentRoundBrasileiraoA = 0;
    //   } else if(dd > 20 && dd <= 27){
    //     var currentRoundBrasileiraoA = 1;
    //   } else if(dd > 27){
    //     var currentRoundBrasileiraoA = 2;
    //   }
    // }

    // if(MM == 5){
    //   if(dd <= 12){
    //     var currentRoundBrasileiraoA = 3;
    //   } else if(dd > 12 && dd <= 18){
    //     var currentRoundBrasileiraoA = 4;
    //   } else if(dd > 18){
    //     var currentRoundBrasileiraoA = 2;
    //   }
    // }
    
    // console.log(dd);

    var currentRoundParanaense = 15;
    var currentRoundCopaBrasil = 0;
    var currentRoundCopaDoMundoJogos = 4;
    var currentRoundCopaDoMundoClassificacao = 0;
    var currentRoundCopaDoMundoBolao = 20;
    var currentRoundBrasileiraoA = 12;
    var currentRoundBrasileiraoB = 14;

    $(".c-slider--table-paranaense").slick('slickGoTo', currentRoundParanaense, true);
    $(".c-slider--table-copa-brasil").slick('slickGoTo', currentRoundCopaBrasil, true);
    $(".c-slider--table-copa-do-mundo-jogos").slick('slickGoTo', currentRoundCopaDoMundoJogos, true);
    $(".c-slider--table-copa-do-mundo-classificacao").slick('slickGoTo', currentRoundCopaDoMundoClassificacao, true);

    $(".c-slider--table-copa-do-mundo-bolao").slick('slickGoTo', currentRoundCopaDoMundoBolao, true);

    $(".c-slider--table-brasileirao-a").slick('slickGoTo', currentRoundBrasileiraoA, true);
    $(".c-slider--table-brasileirao-b").slick('slickGoTo', currentRoundBrasileiraoB, true);

    // Remove o texto das setas
    $('.slick-slider').each(function(){
      $(this).find('.slick-arrow').text('');
    });


  });
})(window, jQuery);

//slider vídeos home
function sliderVideos(){
  $('.c-slider--videos').slick({
    slidesToShow: 4,
    slidesToScroll: 4,
    adaptiveHeight: true,
    responsive: [
      {
        breakpoint: 1300,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          dots: true
        }
      },
      {
        breakpoint: 720,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          dots: true
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          dots: true,
          arrows: false,
        }
      }
    ]
  });
}

function sliderHeadlinesBg(){
  $('.c-slider--headlines-bg-color').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    //fade: true,
    infinite: false,
    adaptiveHeight: true,
    responsive: [
      {
        breakpoint: 500,
        settings: {
          dots: true,
          arrows: false,
        }
      }
    ]
  });
}

function sliderTable(){
  $('.c-slider--table').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    fade: true,
    infinite: false,
    adaptiveHeight: true,
    responsive: [
      {
        breakpoint: 500,
        settings: {
          dots: true,
          arrows: false,
        }
      }
    ]
  });
}

function sliderBolao(){
  $('.c-slider--table-copa-do-mundo-bolao').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    fade: true,
    infinite: false,
    adaptiveHeight: true,
    responsive: [
      {
        breakpoint: 764,
        settings: {
          dots: true,
          arrows: true,
        }
      }
    ]
  });
}