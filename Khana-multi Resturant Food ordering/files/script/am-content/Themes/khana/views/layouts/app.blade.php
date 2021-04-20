<!doctype html>
<html class="no-js" lang="{{ App::getlocale() }}">


<head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    {!! SEO::generate() !!}
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="shortcut icon" type="image/x-icon" href="{{ asset('uploads/favicon.ico') }}">
    <!-- Place favicon.ico in the root directory -->

    <!-- CSS here -->
    @if(Session::has('lang_position'))
    @if(Session::get('lang_position') == 'RTL')
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/bootstrap-rtl.css') }}">
    @else 
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/bootstrap.min.css') }}">
    @endif
    @else 
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/bootstrap.min.css') }}">
    @endif
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/owl.carousel.min.css') }}">
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/fontawesome-all.min.css') }}">
    <link rel="stylesheet" href="{{ theme_asset('khana/public/fonts/themify-icons/themify-icons.css') }}">
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@300;400;500;600;700;800;900&family=Righteous&display=swap" rel="stylesheet">
    <link href="{{ theme_asset('khana/public/css/hc-offcanvas-nav.css') }}" rel="stylesheet"/>
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/default.css') }}">
    @php
        $color = App\Options::where('key','color')->first();
        if(isset($color))
        {
            $theme_color = $color->value;
        }else{
            $theme_color = '#ff3252';
        }
    @endphp
    <style>
        :root{
            --theme-color: {{ $theme_color }};
        }
    </style>
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/style.css') }}">
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/responsive.css') }}">
    @if(Session::has('lang_position'))
    @if(Session::get('lang_position') == 'RTL')
    <link rel="stylesheet" href="{{ theme_asset('khana/public/css/rtl.css') }}">
    @endif
    @endif
    @stack('css')

</head>

<body>
    <!--[if lte IE 9]>
            <p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="https://browsehappy.com/">upgrade your browser</a> to improve your experience and security.</p>
        <![endif]-->


    <!-- header area start -->
    @include('theme::layouts.partials.header')
    <!-- header area end -->

    <div id="pjax-container">
        @yield('content')
    </div>
    
    <!-- footer area start -->
    @include('theme::layouts.partials.footer')
    <!-- footer area end -->

    <!-- JS here -->
    <script src="{{ theme_asset('khana/public/js/vendor/jquery-3.5.1.min.js') }}"></script>
    @stack('js')
    <script src="{{ theme_asset('khana/public/js/store/store.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/store/cart.js') }}"></script>

    <script src="{{ theme_asset('khana/public/js/popper.min.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/bootstrap.min.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/owl.carousel.min.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/hc-offcanvas-nav.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/simpler-sidebar.js') }}"></script>
    <script src="{{ theme_asset('khana/public/js/main.js') }}"></script>
</body>

</html>