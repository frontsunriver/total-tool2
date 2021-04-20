<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
        <meta name="description" content="">
        <meta name="author" content="">
        <link rel="icon" href="{{ asset('../../favicon.ico') }}">
        <title>Admin</title>
        <link href="{{ asset('/css/bootstrap.min.css') }}" rel="stylesheet">
        <link href="{{ asset('/summernote/summernote-bs4.css') }}" rel="stylesheet"> 
        <link href="{{ asset('/css/instant.css') }}" rel="stylesheet">
        <link href="{{ asset('/css/simple-line-icons.css') }}" rel="stylesheet">
    </head>
    <body>
        @include('layouts.adminnav')

        @include('layouts.jumbotronadmin')

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

        @include('layouts.adminfooter')
    </body>
</html>