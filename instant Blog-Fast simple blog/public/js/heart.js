$(document).ready(function(){

    $(".heart").click(function(){
    var CSRF_TOKEN = $('meta[name="csrf-token"]').attr('content');
    var getMediaId = $(this).attr("id").split("heart");
    var messageID = getMediaId[1];
    var DataLink = $(this).attr("data-link");
    $(this).css("background-position","");
    var relStyle = $(this).attr("rel");
    var url = DataLink + "/" + messageID + "/click";

    if(relStyle === 'like') 
    {      
      $(this).addClass("heartAnimation").attr("rel","unlike");        
    }else
    {
      $(this).removeClass("heartAnimation").attr("rel","like");
      $(this).css("background-position","left");
    }

    $.ajax({
    type: "POST",
    url: url,
    cache: false,
    data: {_token: CSRF_TOKEN},
    dataType: 'JSON',
    success: function(data){
        console.log(data);
        $("#likeCount"+messageID).html(data);
      }
    });
    });
});