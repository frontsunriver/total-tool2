@extends('layouts.master')
@section('bodyclass')
    <body class="bg-instant">
@endsection
@section('jumbotron')
<div class="jumbotron mb-2">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-md-5 my-2">
                <div class="profile-card">
                    @if (substr( $user->avatar, 0, 4 ) === "http")
                    <img class="profile-card-photo" src=" {{ $user->avatar }} ">
                    @else
                    <img class="profile-card-photo" src="{{ url('/images/' . $user->avatar) }}">
                    @endif
                </div>

                <div class="profile-card">
                    <div class="profile-card-name">{{ $user->name }}</div>
                    <div class="profile-card-username">
                        {{ $user->username }}
                        @isset($user->role)
                        <img src="{{ url('/images/patch-check-fill.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                        @endisset
                    </div>
                    <div class="profile-card-points"> @lang('messages.user.level') : <b>{{ levelNumber($point->sum('likes_count')) }}</b> @lang('messages.user.points') : <b>{{ $point->sum('likes_count') }}</b> 
                    </div>
                    @isset($user->role)
                    <div><label class="badge badge-light">@lang($user->role)</label></div>
                    @endisset
                </div>
            </div>
            <div class="col-md-4">
                <ul class="profile-links-list">
                    @isset($user->website)
                    <li class="nowrap">
                    <a  role="button" class="btn btn-light btn-sm btn-block" target="_blank" data-toggle="tooltip" data-placement="bottom" title="Website" href="{{ $user->website }}">
                        <i class="icon-link icons"></i></a>
                    </li>
                    @endisset
                    @isset($user->facebook)
                    <li class="nowrap">
                    <a  role="button" class="btn btn-light btn-sm btn-block" target="_blank" data-toggle="tooltip" data-placement="bottom" title="Facebook" href="https://www.facebook.com/{{ $user->facebook }}">
                        <i class="icon-social-facebook icons"></i></a>
                    </li>
                    @endisset
                    @isset($user->twitter)
                    <li class="nowrap">
                    <a  role="button" class="btn btn-light btn-sm btn-block" target="_blank" data-toggle="tooltip" data-placement="bottom" title="Twitter" href="https://twitter.com/{{ $user->twitter }}">
                        <i class="icon-social-twitter icons"></i></a>
                    </li>
                    @endisset
                    @isset($user->instagram)
                    <li class="nowrap">
                    <a  role="button" class="btn btn-light btn-sm btn-block" target="_blank" data-toggle="tooltip" data-placement="bottom" title="Instagram" href="https://www.instagram.com/{{ $user->instagram }}">
                        <i class="icon-social-instagram icons"></i></a>
                    </li>
                    @endisset
                    @isset($user->linkedin)
                    <li class="nowrap">
                    <a  role="button" class="btn btn-light btn-sm btn-block" target="_blank" data-toggle="tooltip" data-placement="bottom" title="LinkEdin" href="{{ $user->linkedin }}">
                        <i class="icon-social-linkedin icons"></i></a>
                    </li>
                    @endisset
                </ul>                    
            </div>
            <div class="col-md-3">            
            @if (Auth::id() == $user->id)
                <a role="button" class="btn btn-light" data-toggle="modal" data-target="#recentLikes">@lang('messages.user.likes')</a>
                <a href="{{ url('/profile/' . $user->username . '/edit/') }}" role="button" class="btn btn-light">@lang('messages.user.edit_profile')</a>
            @endif
            </div>
        </div>
    </div>
</div>
@endsection
@section('content')
<div class="se-pre-con"></div>
<div class="container-fluid mt-5">
    <div class="row mt-2">
        <div class="grid" data-columns>
            @forelse($posts as $key => $post)      
            @include('public.post')
            @empty
            <div class="col-md-12">
                <h5 class="text-light">@lang('messages.nopost')</h5>
            </div>
            @endforelse    
        </div>
    </div>
    <hr>
    <div class="row">
        <div class="col-md-12">
            {{ $posts->links() }}
        </div>
    </div>  
</div>
<div class="modal fade" id="recentLikes" tabindex="-1" role="dialog" aria-labelledby="recentLikes" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header border-0">
                <h5 class="modal-title" id="exampleModalLongTitle">@lang('messages.user.likes')</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="list-group">
                    @forelse($userlikes as $feed)
                    <a href="{{url('/posts/' . $feed->post_slug)}}" class="list-group-item list-group-item-action flex-column align-items-start">
                        <div class="d-flex w-100 justify-content-between">
                            <h5 class="mb-1">{{ str_limit($feed->post_title, 35) }}</h5>
                            <small class="text-muted">@lang('messages.user.likedby')</small>
                        </div>
                        @forelse ($feed->likes as $user)
                        <small class="text-muted">
                            {{ $user->username }}, 
                        </small>
                        @empty
                        <small class="text-muted">@lang('messages.user.noneliked')</small>
                        @endforelse
                    </a>
                    @empty
                    <h5>
                        @lang('messages.user.nofeed')
                    </h5>
                    @endforelse                         
                </div>
            </div>
            <div class="modal-footer border-0">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">@lang('messages.user.close')</button>
            </div>
        </div>
    </div>
</div>
@endsection