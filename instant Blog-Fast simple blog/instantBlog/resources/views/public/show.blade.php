@extends('layouts.mastershow')
@section('content')
<div class="container mt-5">
    <div class="row">
        <div class="col-md-8">     
            <div class="card">
                @auth
                @can('moderator-post', $post)
                <div class="card-header">
                    <div class="row">
                        <div class="col-8 text-muted">
                        <small>
                        <strong>@lang('messages.loged') {{ Auth::user()->role }}</strong>
                        @isset($editby)
                            @lang('messages.editby') {{ $editby->username }} - {{ $post->updated_at->diffForHumans() }}
                        @endisset
                        </small>
                        </div>
                        <div class="col">
                            <a href="{{ url('/home/' . $post->id . '/edit') }}" class="btn btn-primary btn-sm float-right btn-white-color" role="button" >@lang('messages.edit')</a>
                        </div>
                        <div class="col">
                            <a href="{{ url('/home/' . $post->id) }}" class="btn btn-danger btn-sm btn-white-color" role="button" >@lang('messages.delete')</a>
                        </div>                        
                    </div>
                </div>
                @elsecan('own-post', $post)
                <div class="card-header">
                    <div class="row">
                        <div class="col-8 text-muted">
                            
                        <small>
                        <strong>{{ $post->user->name }} @lang('messages.yourpost')</strong>
                        @isset($editby)
                            @lang('messages.editby') {{ $editby->username }} - {{ $post->updated_at->diffForHumans() }}
                        @endisset
                        </small>
                        </div>
                        <div class="col">
                            <a href="{{ url('/home/' . $post->id . '/edit') }}" class="btn btn-primary btn-sm float-right btn-white-color" role="button" >@lang('messages.edit')</a>
                        </div>
                        <div class="col">
                            <a href="{{ url('/home/' . $post->id) }}" class="btn btn-danger btn-sm btn-white-color" role="button" >@lang('messages.delete')</a>
                        </div>
                    </div>
                </div>
                @endcan
                @endauth
                @if (!empty($post->post_video))
                    <div class="embed-responsive embed-responsive-16by9 mb-3 card-img-top">
                      <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/{{ $post->post_video }}" allowfullscreen></iframe>
                    </div>
                @elseif(!empty($post->post_media))
                    <img class="card-img-top img-fluid" src="{{ url('/uploads/' . $post->post_media) }}">
                @endif
                <div class="card-body">
                    <div class="list-item mb-3">
                        <div class="list-left">
                            <a href="{{ url('/profile/' .$post->user->username) }}">
                            @if (substr( $post->user->avatar, 0, 4 ) === "http")
                            <img class="avatar img-fluid rounded-circle" src="{{ $post->user->avatar }}">
                            @else
                            <img class="avatar img-fluid rounded-circle" src="{{ url('/images/' . $post->user->avatar) }}">
                            @endif
                            </a>
                        </div>         
                        <div class="list-body">
                            <div class="text-ellipsis">
                                <a class="nocolor" href="{{ url('/profile/' .$post->user->username) }}">{{ $post->user->name }}</a>
                                <small class="text-muted time">{{ $post->created_at->diffForHumans() }}</small>
                            </div>
                            <div class="text-ellipsis">
                                <small class="text-muted">
                                    {{ $post->user->username }}
                                </small>
                                @isset($post->user->role)
                                <img src="{{ url('/images/patch-check-fill.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                                @endisset
                                @if (count($post->tags))
                                @foreach ($post->tags as $tag)
                                <small class="text-muted time"><a href="{{ url('/category/' . $tag->name) }}">
                                    #{{ $tag->name }}
                                </a></small>
                                @endforeach
                                @endif
                            </div>
                        </div>
                    </div>
                    <h1>{{ $post->post_title }}</h1>
                    @if (!empty($post->post_desc))
                    <p>
                        {{ $post->post_desc }}
                    </p>
                    @endif
                    @if (!empty($setting->post_ads))
                    <p>
                        {!! $setting->post_ads !!}
                    </p>
                    @endif
                @if($post->contents)
                    @foreach ($post->contents as $content)
                    @if ($content->type == "header")
                    <h4>{{ $content->body }}</h4>
                    @endif

                    @if ($content->type == "text")
                    <p>{{ $content->body }}</p>
                    @endif

                    @if ($content->type == "txteditor")
                    {!! clean( $content->body ) !!}
                    @endif

                    @if ($content->type == "image")
                    <img class="img-fluid mb-3" src="{{ url('/uploads/' . $content->body) }}">
                    @endif

                    @if ($content->type == "youtube")
                    <div class="embed-responsive embed-responsive-16by9 mb-3">
                        <iframe class="embed-responsive-item" src="https://www.youtube.com/embed/{{ $content->body }}" allowfullscreen></iframe>
                    </div>
                    @endif

                    @if ($content->type == "tweet")
                    <div class="embedbox mx-auto mb-3">
                        {!! $content->embed->embedcode !!}
                    </div>
                    @endif

                    @if ($content->type == "facebook")
                    <div class="embedbox mx-auto mb-3">
                        <div class="fb-post" 
                        data-href="{{ $content->embed->url }}"
                        data-width="auto"></div>
                    </div>
                    @endif

                    @if ($content->type == "instagram")
                    <div class="embedbox mx-auto mb-3">
                        {!! $content->embed->embedcode !!}
                    </div>
                    @endif

                    @if ($content->type == "pinterest")
                    <div class="embedbox mx-auto mb-3">
                        <a data-pin-do="embedPin" data-pin-width="medium" href="{{ $content->embed->url }}"></a>
                    </div>
                    @endif
                    @endforeach
                @endif
                </div>
                <div class="card-body card-border">
                    <div class="row">
                        <div class="col like ml-3 lesspadding">
                            @if (Auth::check())
                            @if ($post->isLiked)
                            <div class="heart heartliked" id="heart{{ $post->id }}" rel="unlike" data-link="{{ url('/post') }}"></div>
                            @else
                            <div class="heart" id="heart{{ $post->id }}" rel="like" data-link="{{ url('/post') }}"></div>
                            @endif
                            @else
                            <a href="{{url('/login/')}}" >
                                <div class="heartguest"></div>
                            </a>
                            @endif
                            <div class="likeCount" id="likeCount{{ $post->id }}">{{ shortNumber($post->likes()->count()) }}</div>
                        </div>
                        <div class="col lesspadding">
                            <a href="https://www.pinterest.com/pin/create/button/"
                               data-pin-do="buttonBookmark"
                               data-pin-tall="true" 
                               data-pin-custom="true">
                                <button type="button" class="btn btn-sm btn-block btn-danger btnpoint"> <i class="icon-social-pinterest icons"></i> <span class="d-none d-md-inline-block">@lang('messages.save')</span></button>
                            </a>
                        </div>
                        <div class="col lesspadding">                            
                            <a role="button" class="btn btn-face btn-sm share" href="https://www.facebook.com/sharer/sharer.php?u={{ url('/posts/' . $post->post_slug) }}" target="_blank"><i class="icon-social-facebook icons"></i> <span class="d-none d-md-inline-block">@lang('messages.share')</span></a>
                        </div>
                        <div class="col lesspadding">
                            <a role="button" class="btn btn-twit btn-sm share" href="https://twitter.com/share?url={{ url('/posts/' . $post->post_slug)}}" target="_blank"><i class="icon-social-twitter icons"></i> <span class="d-none d-md-inline-block">@lang('messages.share')</span></a>
                        </div>
                    </div>   
                </div>
                @if ($setting->allow_comments == '0')
                <div class="card-footer">
                    <div class="fb-comments" data-href="{{ url('/posts/' . $post->post_slug) }}" data-width="100%" data-numposts="5"></div>
                </div>
                @endif
            </div>
        </div>
        <div class="col-md-4">
            <div class="row mb-5">
                <div class="col">
                @if($previous)
                <a role="button" class="btn btn-block btn-arrow border-one" href="{{url('/posts/' . $previous)}}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.previous')">
                    <i class="icon-arrow-left icons"></i>
                </a>
                @endif
                </div>
                <div class="col">
                @if($random)
                <a role="button" class="btn btn-block btn-arrow border-one" href="{{url('/posts/' . $random)}}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.random')">
                    <i class="icon-shuffle icons"></i>
                </a>
                @endif
                </div>
                <div class="col">
                @if($next)
                <a role="button" class="btn btn-block btn-arrow border-one" href="{{url('/posts/' . $next)}}" data-toggle="tooltip" data-placement="bottom" title="@lang('messages.next')">
                    <i class="icon-arrow-right icons"></i>
                </a>
                @endif
                </div>
            </div>
            @if (!empty($setting->page_ads))
            <div class="card pagesideads embed-responsive mb-3">
                {!! $setting->page_ads !!}
            </div>
            @endif
            @forelse($related as $relatedpost)
            @include('public.relatedpost')
            @empty
            <h6 class="text-light text-center">@lang('messages.norelated')</h6>
            @endforelse
        </div>
    </div>
</div>
@endsection
@section('extra')
<footer class="blog-footer">
@if (count($pages) > 0)
    <ul class="list-inline">
    @foreach ($pages as $page)    
       <li class="list-inline-item"><a class="text-light" href="{{url('/page/' . $page->page_slug)}}">{{ $page->page_title }}</a></li>
    @endforeach
    </ul>
@endif
@if (!empty($setting->footer))
<div class="text-muted foottxt">{!! clean($setting->footer) !!}</div>
@endif
</footer>
@endsection