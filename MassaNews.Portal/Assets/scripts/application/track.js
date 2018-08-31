$(function () {
  "use strict";

  /*
   * TRACK BLOGS HOME (DESTAQUE + SIDEBAR)
   */
  $(document).on('click', '.track-blogs-home a', function (event){
    var blogName     = $(this).parents('.item').find('.author').text();
    var blogCategory = $(this).parents('.item').find('.hat').text();
    var position     = $(this).data('position');
    var link         = $(this).attr('href');
    var trackText;
    var eventValue;

    var isBlogsDestaque = $(this).parents('.track-blogs-home').hasClass('track-blogs-home-destaque');
    var isBlogsSidebar  = $(this).parents('.track-blogs-home').hasClass('track-blogs-home-sidebar');

    if(isBlogsDestaque){
      eventValue = "blogs-home-destaque"
    } else if(isBlogsSidebar){
      eventValue = "blogs-home-sidebar";
    }

    if($(this).hasClass('hat')){
      trackText = "Categoria (" + blogCategory + ")";
    } else if($(this).hasClass('img')){
      trackText = "Imagem";
    } else if($(this).hasClass('author')){
      trackText = "Nome";
    } else if($(this).hasClass('post')){
      trackText = "Posição: " + position + " // Link: " + link;
    }

    if(typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'track-action' : blogName,
        'track-label'  : trackText,
        'event'        : eventValue
      });
    }

    // console.log(eventValue);
    // event.preventDefault();
  });


  /*
   * TRACK HEADLINES HOME
   */
  $(document).on('click', '.track-headlines-home a', function (event){
    var sectionTitle    = $(this).parents('.track-headlines-home').find('.c-category-title .title').text();
    var isSubLink       = $(this).hasClass('sublink');
    var isSeeAll        = $(this).hasClass('link-see-all');
    var isTitleSection  = $(this).hasClass('title-section');
    var elementPosition = $(this).data('position');
    var elementLink     = $(this).attr('href');
    var elementClass    = $(this).parent().attr('class');
    var newsPosition    = $(this).parents('.c-headline').data('position');
    var typeSection     = $(this).parents('.track-headlines-home');
    var isFeatured      = typeSection.hasClass('track-headlines-featured-home');
    var isSections      = typeSection.hasClass('track-headlines-sections-home');
    var isCategories    = typeSection.hasClass('track-headlines-categories-home');

    var sectionName;
    var trackLabel;
    var eventValue;

    if(isFeatured){
      sectionName = "Destaques";
      eventValue = 'headlines-featured-home';
    } else if(isSections){
      sectionName = sectionTitle;
      eventValue = 'headlines-sections-home';
    } else if(isCategories){
      sectionName = sectionTitle;
      eventValue = 'headlines-categories-home';
    }

    if(isTitleSection){
      trackLabel = "Título: " + elementLink;
    } else if(isSubLink){
      trackLabel = "Sublink " + elementPosition + ": " + elementLink;
    } else if(isSeeAll){
      trackLabel = "Ver mais (+): " + elementLink;
    } else {
      trackLabel = "Posição: " + newsPosition + " // " + elementClass + " // " + "Link: " + elementLink;
    }

    if(typeof dataLayer !== 'undefined') {
      dataLayer.push({
        'track-action': sectionName,
        'track-label' : trackLabel,
        'event'       : eventValue
      });
    }

    // console.log(eventValue, sectionName, trackLabel);
    // event.preventDefault();
  });

});
