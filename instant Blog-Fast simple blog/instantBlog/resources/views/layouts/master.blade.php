<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="{{ !empty($tag->desc) ? $tag->desc : $setting->site_desc }}">
        <meta name="csrf-token" content="{{ csrf_token() }}">
        <meta property="fb:pages" content="{{ env('FACEBOOK_PAGE_ID') }}" />
        <link rel="icon" type="image/png" href="{{ asset('favicon.png') }}">
        <title>{{ $setting->site_name . ' - ' . $setting->site_title }}</title>        
        <link href="{{ asset('/css/bootstrap.min.css') }}" rel="stylesheet">
        <link href="{{ asset('/css/instant.css') }}" rel="stylesheet">
        <link href="{{ asset('/css/simple-line-icons.css') }}" rel="stylesheet">
        @yield('css')
    </head>
    @yield('bodyclass')

        @include('layouts.nav')

        @yield('jumbotron')

        @if ($flash = session('message'))
        <div class="container">
            <div class="alert alert-success" role="alert">
                {{ $flash }}
            </div>
        </div>
        @elseif ($flash = session('error'))
        <div class="container">
            <div class="alert alert-danger" role="alert">
                {{ $flash }}
            </div>
        </div>
        @endif

        @yield('content')

        @yield('extra')
        
        @include('layouts.footer')

        @stack('scripts')
    </body>
</html>