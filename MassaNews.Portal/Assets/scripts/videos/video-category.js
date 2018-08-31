(function (){
  'use strict';

  // Document.ready
  window.addEventListener('DOMContentLoaded', function (){
    $(window).scroll(function (){
      var trueHeight = $('body')[0].scrollHeight;
      var divHeight = $(window).height();
      var scrollLeft = trueHeight - divHeight;
      var percent = ($(this).scrollTop() * 100) / scrollLeft;

      if (percent >= 50){
        LoadNexNavegationLink();
      }
    });
  });
})();

var lastRequestDate = "";

function LoadNexNavegationLink(){
  var lastDate = $("#sections .t-module").last().attr('data-date');

  if (lastRequestDate != lastDate){
    lastRequestDate = lastDate;

    var category = $(".g-main").attr('data-category');
    var url = "/Videos/LoadNexCategoryLinks";

    $("<div>").load(url, { lastDate: lastDate, category: category }, function (response, status, xhr){
      $("#sections").append($(this));
    });
  }
  return false;
}