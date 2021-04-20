<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="{{ !empty($post->post_desc) ? $post->post_desc : $setting->site_desc }}">
        <meta name="csrf-token" content="{{ csrf_token() }}">
        <link rel="icon" type="image/png" href="{{ asset('favicon.png') }}">
        <link rel="amphtml" href="{{ url('posts/' . $post->post_slug) }}/amp">
        <title>{{ !empty($post->post_title) ? $setting->site_name . ' - ' .$post->post_title : $setting->site_name . ' - ' . $setting->site_title }}</title>
        <!-- Facebook og-->
        <meta property="fb:app_id" content="{!! env('FACEBOOK_API_ID') !!}" />
        <meta property="og:url" content="{{ Request::url() }}" />
        <meta property="og:type" content="article" />
        <meta property="og:title" content="{{ !empty($post->post_title) ? $post->post_title : $setting->site_name . '-' . $setting->site_title }}" />
        <meta property="og:description" content="{{ !empty($post->post_desc) ? $post->post_desc : $setting->site_desc }}" />
        @if (!empty($post->post_media))
        <meta property="og:image" content="{{ url('/uploads/' . $post->post_media) }}" />
        @elseif (!empty($post->post_video))
        <meta property="og:image" content="https://i.ytimg.com/vi/{{ $post->post_video }}/hqdefault.jpg"/>
        @else
        <meta property="og:image" content="{{ url('/images/social.png') }}" />
        @endif        
        <!-- Twitter card-->
        <meta name="twitter:card" content="summary_large_image">
        <meta name="twitter:site" content="{{'@'. $setting->tw_user }}" />
        <meta name="twitter:creator" content="{{'@'. $setting->tw_user }}">
        <meta name="twitter:url" value="{{ Request::url() }}" />
        <meta name="twitter:title" content="{{ !empty($post->post_title) ? $post->post_title : $setting->site_name . '-' . $setting->site_title }}" />
        <meta name="twitter:description" content="{{ !empty($post->post_desc) ? $post->post_desc : $setting->site_desc }}" />
        @if (!empty($post->post_media))
        <meta name="twitter:image" content="{{ url('/uploads/' . $post->post_media) }}" />
        @elseif (!empty($post->post_video))
        <meta name="twitter:image" content="https://i.ytimg.com/vi/{{ $post->post_video }}/hqdefault.jpg"/>
        @else
        <meta name="twitter:image" content="{{ url('/images/social.png') }}" />
        @endif       
        <!-- Css styles-->
        <link href="{{ asset('/css/bootstrap.min.css') }}" rel="stylesheet">
        <link href="{{ asset('/css/instant.css') }}" rel="stylesheet">  
        <link href="{{ asset('/css/simple-line-icons.css') }}" rel="stylesheet">
    </head>
    <body class="bg-instant">
        @include('layouts.nav')

        @yield('content')

        <div id="fbcomment" data-user-id="{{ env('FACEBOOK_API_ID') }}"></div>

        @yield('extra')
        
        @include('layouts.footer')

        <script async src="https://platform.twitter.com/widgets.js"></script>
        <script async defer src="//platform.instagram.com/en_US/embeds.js"></script>
        <script async defer src="//assets.pinterest.com/js/pinit.js"></script>
        <script>
        $(document).ready(function() {
            $('.share').click(function(e) {
                e.preventDefault();
                window.open($(this).attr('href'), 'fbShareWindow', 'height=450, width=550, top=' + ($(window).height() / 2 - 275) + ', left=' + ($(window).width() / 2 - 225) + ', toolbar=0, location=0, menubar=0, directories=0, scrollbars=0');
                return false;
            });

            window.fbAsyncInit = function() {
              var userId = $('#fbcomment').data("user-id");
              FB.init({
                appId            : userId,
                autoLogAppEvents : true,
                xfbml            : true,
                version          : 'v2.11'
              });
            };

            (function(d, s, id){
               var js, fjs = d.getElementsByTagName(s)[0];
               if (d.getElementById(id)) {return;}
               js = d.createElement(s); js.id = id;
               js.src = "https://connect.facebook.net/en_US/sdk.js";
               fjs.parentNode.insertBefore(js, fjs);
             }(document, 'script', 'facebook-jssdk'));
        });
        </script>
    </body>
</html>