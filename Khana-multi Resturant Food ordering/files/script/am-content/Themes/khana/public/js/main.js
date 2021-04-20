(function($) {
     "use strict";
       // owlCarousel
    $('.restaurants').owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        responsive: {
            0: {
                items: 2
            },
            450: {
                items: 3
            },
            767: {
                items: 4
            },
            992: {
                items: 7
            }
        }
    });

    // owlCarousel
    $('.best_rated').owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: false,
        responsive: {
            0: {
                items: 1
            },
            767: {
                items: 3
            },
            992: {
                items: 4
            }
        }
    });

    

    // owlCarousel
    $('.recent_sell_restaurants').owlCarousel({
        loop: true,
        items: 7,
        dots: false,
        responsiveClass:true,
        responsive: {
            0: {
                items: 4
            },
            250: {
                items: 2
            },
            350: {
                items: 3
            },
            767: {
                items: 6
            },
            1000: {
                items: 6
            }
        }
    });

    $('.notify-close-btn').on('click','a',function(){
        var url = $('#header_close').val();
        $.ajax({
            type: 'GET',
            url: url,
            dataType: 'html',
            success: function(response){ 
                $('.header-notify-bar').fadeOut();
            }
        })
    });



    $('.gallery').owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        nav: true,
        dots: false,
        responsive: {
            0: {
                items: 1
            },
            767: {
                items: 1
            },
            992: {
                items: 1
            }
        }
    })

    $('.store-inline-action ul').on('click', 'li', function() {
        $(this).addClass('active').siblings().removeClass('active');
    });


    

   

    

    /*----------------------------
        Jquery Live Search
      ------------------------------*/
    $('#restaurants_search').keyup(function(){  
      search_table($(this).val());  
    });  
    function search_table(value){  
        $('.filter-right-section .col-lg-4').each(function(){  
        var found = 'false';  
        $(this).each(function(){  
          if($(this).text().toLowerCase().indexOf(value.toLowerCase()) >= 0)  
          {  
            found = true;  
          }  
        });  
        if(found == true)  
        {  
          $(this).show();  
        }  
        else  
        {  
          $(this).hide();  
        }  
      });  
    }

    $('#user_settings_form').on('submit',function(e){
        e.preventDefault();
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            type: 'POST',
            url: this.action,
            data: new FormData(this),
            dataType: 'json',
            contentType: false,
            cache: false,
            processData:false,

            success: function(response){ 
                if(response.error)
                {
                    $('.error-message-area').fadeIn();
                    $('.error-msg').html(response.error);
                    $(".error-message-area").delay( 2000 ).fadeOut( 2000 );
                }

                if(response == 'ok')
                {
                    $('.alert-message-area').fadeIn();
                    $('.ale').html('Settings successfully updated');
                    $(".alert-message-area").delay( 2000 ).fadeOut( 2000 );
                }
            }
        })
    });

    $('.media-img').on('change',function(){
        var uploadFile = $(this);
        var files = !!this.files ? this.files : [];
        if (!files.length || !window.FileReader) return; 
        var reader = new FileReader();
        reader.readAsDataURL(files[0]); 

        reader.onloadend = function(){ 
            uploadFile.closest(".imgUp").find('.imagePreview').css("background-image", "url("+this.result+")");
            
        }
    });


    $('#select_language').on('change',function(e){
        e.preventDefault();
        var locale = $('#select_language option:selected').val();
        var url = $('#lang_url').val();
        $.ajax({
            type: 'GET',
            url: url,
            data: {locale:locale},
            dataType: 'html',

            success: function(response){ 
                location.reload();
            }
        })
    });

    $("#sidebar-left").simplerSidebar({
          align: "right",
          selectors: {
            trigger: "#toggle-sidebar-left",
        }
    });


    $('#toggle-sidebar-left').on('click',function(){
      $('.sidebar').removeClass('d-none');
    });


    //navbar mobile menu
    var $main_nav = $('#main-nav');
      var $toggle = $('.toggle');

      var defaultOptions = {
        disableAt: false,
        customToggle: $toggle,
        levelSpacing: 40,
        navTitle: 'Menu',
        levelTitles: true,
        levelTitleAsBack: true,
        pushContent: '#container',
        insertClose: 2
      };

      // call our plugin
      var Nav = $main_nav.hcOffcanvasNav(defaultOptions);

      // add new items to original nav
      $main_nav.find('li.add').children('a').on('click', function() {
        var $this = $(this);
        var $li = $this.parent();
        var items = eval('(' + $this.attr('data-add') + ')');

        $li.before('<li class="new"><a href="#">'+items[0]+'</a></li>');

        items.shift();

        if (!items.length) {
          $li.remove();
        }
        else {
          $this.attr('data-add', JSON.stringify(items));
        }

        Nav.update(true);
      });

      // demo settings update

      const update = (settings) => {
        if (Nav.isOpen()) {
          Nav.on('close.once', function() {
            Nav.update(settings);
            Nav.open();
          });

          Nav.close();
        }
        else {
          Nav.update(settings);
        }
      };

      $('.actions').find('a').on('click', function(e) {
        e.preventDefault();

        var $this = $(this).addClass('active');
        var $siblings = $this.parent().siblings().children('a').removeClass('active');
        var settings = eval('(' + $this.data('demo') + ')');

        update(settings);
      });

      $('.actions').find('input').on('change', function() {
        var $this = $(this);
        var settings = eval('(' + $this.data('demo') + ')');

        if ($this.is(':checked')) {
          update(settings);
        }
        else {
          var removeData = {};
          $.each(settings, function(index, value) {
            removeData[index] = false;
          });

          update(removeData);
        }
      });

    $(window).load(function()
    {
        $('#staticBackdrop').modal('show');
    });

    /*----------------------------
        Ajax Contact Form Submit
        ------------------------------*/
    $('#contact_form').on('submit',function(e){
        e.preventDefault();
        $.ajaxSetup({
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            }
        });
        $.ajax({
            type: 'POST',
            url: this.action,
            data: new FormData(this),
            dataType: 'json',
            contentType: false,
            cache: false,
            processData:false,
            beforeSend: function() {
                $('#contact_button_label').html('Sending...');
            },
            success: function(response){ 
                $('#contact_button_label').html('Send Message');
                $('#success').html('Your Message Send Successfully');
                $('#name').val('');
                $('#email').val('');
                $('#subject').val('');
                $('#message').val('');
            },
            error: function(xhr, status, error) 
            {
                $('#error').html('Something Went Wrong');
                $('#contact_button_label').html('Send Message');
            }
        })


    });

})(jQuery);
function resize(s,size) {

    var ext= s.substring(s.lastIndexOf("."));
    
    var new_string = s.substring(0, s.lastIndexOf(".")) + size + s.substring(s.lastIndexOf("."));

    return new_string;
}

var l = 1;
function addon_minus()
{
 
    if($('#qty_value_input').val() > 1)
    {
        l--;
        $('.qty-value').html(l);
        $('#qty_value_input').val(l);
    }
    
}

function addon_plus()
{
    l++;
    $('.qty-value').html(l);
    $('#qty_value_input').val(l);
}

function select_payment(value)
{
    $('.single-payment-section').removeClass('active');
    $('.'+value).addClass('active');
}