@extends('layouts.master')
@section('css')
<style type="text/css">
    .bg-nav {
        margin-top: 0px!important;
        padding-top: 1.35rem;
        background: linear-gradient(to right bottom, #3f4756, #4a4a69, #634874, #834074, #a23567);
    }
</style>
@endsection
@section('bodyclass')
<body>
@endsection
@section('jumbotron')
    <div class="jumbotron jblight">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-md-9">
                    <h1 class="display-4">@lang('messages.home.title')</h1>
                    @isset(Auth::user()->role)
                    <h6>@lang('messages.home.youare') @lang(Auth::user()->role)</h6>
                    @endisset
                </div>
                <div class="col-md-3">
                    <div class="admin-item-img">
                        <a href="{{ url('/profile/' . Auth::user()->username) }}">
                            @if (substr( Auth::user()->avatar, 0, 4 ) === "http")
                            <img src="{{ Auth::user()->avatar }}" class="admin-image rounded-circle">
                            @else
                            <img src="{{ url('/images/' . Auth::user()->avatar) }}" class="admin-image rounded-circle">
                            @endif
                        </a>
                    </div>
                    <a href="{{ url('/profile/' . Auth::user()->username) }}">
                        <p class="member-item-user">{{ Auth::user()->name }}</p>
                    </a>
                    <p class="member-item-text">{{ Auth::user()->username }}</p>
                </div>
            </div>
        </div>
    </div>
    @endsection
    @section('content')
    <div class="container">
        @can('admin-area')
        <div class="row ml-3 mr-3">
            <div class="col-12">
                <section class="admin admin-simple-sm p-3">
                    <h6> @lang('messages.home.logged') <a href="{{ url('/admin') }}">@lang('messages.home.admin')</a></h6>
                </section>
            </div>
        </div>
        @endcan
        <div class="row m-3">
            @modorall
            <div class="col-md-4">
                <a  href="{{ url('/home/add') }}" role="button" class="btn btn-lg btn-light btn-block btnhome mr-1 mb-2"> <i class="icon-plus icons"></i> <br> @lang('messages.home.addpost')</a>
            </div>
            @endmodorall
            <div class="col-md-4">
                <a href="{{ url('/profile/' . Auth::user()->username) }}" role="button" class="btn btn-lg btn-light btn-block btnhome mr-1 mb-2"><i class="icon-user icons"></i> <br>@lang('messages.home.profile')</a>
            </div>
            <div class="col-md-4">
                <a href="{{ url('/') }}" role="button" class="btn btn-lg btn-light btn-block btnhome mr-1 mb-2"><i class="icon-home icons"></i> <br>@lang('messages.home.homepage')</a>
            </div>
        </div>
        @if (!empty($posts->items()))
        <div class="row ml-3 mr-3">
            <div class="col-12">
                <section class="admin admin-simple-sm p-3">
                    <h6 class="text-secondary mb-4"> @lang('messages.home.unpublished') </h6>
                    <table class="table table-sm table-striped">
                      <form action="{{ url('/cnt/multiple/') }}" method="POST">
                        @csrf
                        {{ method_field('POST') }}
                        <tbody>
                            @forelse($posts as $post)
                            <tr>
                                <td>
                                    <a href="{{url('/posts/' . $post->post_slug)}}" target="_blank">
                                        @if (! empty($post->post_title))
                                        {{ str_limit($post->post_title, 35) }}
                                        @else
                                        @lang('messages.home.notitle')
                                        @endif
                                    </a>
                                </td>
                                <td>
                                    @can('moderator-post')
                                    <a href="{{ url('/profile/' . $post->user->username) }}" target="_blank">{{ str_limit($post->user->name, 10) }}</a>                    
                                    @else
                                    <small class="font-italic text-muted">@lang('messages.home.awaiting')</small>
                                    @endcan
                                </td>
                                <td><a href="{{ url('/home/' . $post->id . '/edit') }}">@lang('messages.edit')</a></td>
                                <td><a class="color-delete" href="{{ url('/home/' . $post->id) }}">@lang('messages.delete')</a></td>
                            </tr>
                            @empty
                            <h5>
                                No Posts Found
                            </h5>
                            @endforelse
                        </tbody>
                    </form>
                </table>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            {{ $posts->links() }}
        </div>
    </div>
    @endif
    </div>
@endsection

@push('scripts')
    <script src="{{ asset('js/form.js') }}"></script>
@endpush