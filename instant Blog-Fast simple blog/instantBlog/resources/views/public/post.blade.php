    @if (!empty($post->post_video))
    <div class="card border-one bg-dark text-white text-center">            
                @if (count($post->tags))
                <div class="category">
                    @foreach ($post->tags as $tag)
                        <a class="{{ $tag->color }}" href="{{ url('/category/' . $tag->name) }}">{{ $tag->name }}</a>
                    @endforeach
                </div>
                @endif
            <div class="bg-over">
            <div class="youtube border-two">
                <span class="play-btn"></span>
                <img class="card-img change-ratio" src="https://i.ytimg.com/vi/{{ $post->post_video }}/hqdefault.jpg">
            </div>
            <a class="link-over" href="{{ url('/posts/' . $post->post_slug )}}"></a>         
            <div class="card-blog-body">              
                <small><i class="icon-clock icons mr-2"></i> {{ $post->created_at->diffForHumans() }}</small>  
                <h4 class="mt-4">{{ str_limit($post->post_title, 70) }}</h4>
                <p class="card-text mb-5">{{ str_limit($post->post_desc, 90) }}</p>
                <a class="author" href="{{ url('/profile/' . $post->user->username) }}">
                    @if (substr( $post->user->avatar, 0, 4 ) === "http")
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ $post->user->avatar }}">
                    @else
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ url('/images/' . $post->user->avatar) }}">
                    @endif
                    <div>
                    {{ $post->user->name }}
                    </div>
                    @isset($post->user->role)
                        <div class="vbadge"> 
                            <img src="{{ url('/images/patch-check-fill-w.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                        </div>
                    @endisset
                </a>
                 <div class="card-like">
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
                <div class="card-count" id="likeCount{{ $post->id }}">{{ shortNumber($post->likes()->count()) }}</div>
                </div>
            </div>
        </div>
    </div>
    @elseif(!empty($post->post_media))
        @if ($post->post_instant == "1")
        <div class="card border-one text-white text-center bg-dark cover" style="background-image: url({{ url('/uploads/' . $post->post_media) }});">
            <div class="card-txt-body bg-over">
                <a class="link-over" href="{{url('/posts/' . $post->post_slug)}}"></a>
                @if (count($post->tags))
                <div class="category">
                    @foreach ($post->tags as $tag)
                        <a class="{{ $tag->color }}" href="{{ url('/category/' . $tag->name) }}">{{ $tag->name }}</a>
                    @endforeach
                </div>
                @endif
                <small><i class="icon-clock icons mr-2"></i> {{ $post->created_at->diffForHumans() }}</small>  
                <h4 class="txt-shad mt-4">{{ str_limit($post->post_title, 50) }}</h4>
                <p class="card-text txt-shad mb-5">{{ str_limit($post->post_desc, 90) }}</p>
                <a class="author" href="{{ url('/profile/' . $post->user->username) }}">
                    @if (substr( $post->user->avatar, 0, 4 ) === "http")
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ $post->user->avatar }}">
                    @else
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ url('/images/' . $post->user->avatar) }}">
                    @endif
                    <div>
                    {{ $post->user->name }}
                    </div>
                    @isset($post->user->role)
                        <div class="vbadge"> 
                            <img src="{{ url('/images/patch-check-fill-w.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                        </div>
                    @endisset
                </a>
                <div class="card-like">
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
                <div class="card-count" id="likeCount{{ $post->id }}">{{ shortNumber($post->likes()->count()) }}</div>
                </div>
            </div>
        </div>
        @else
        <div class="card border-one bg-dark text-white text-center">            
                @if (count($post->tags))
                <div class="category">
                    @foreach ($post->tags as $tag)
                        <a class="{{ $tag->color }}" href="{{ url('/category/' . $tag->name) }}">{{ $tag->name }}</a>
                    @endforeach
                </div>
                @endif
            <div class="bg-over">
            <img class="card-img border-two" src="{{ url('/uploads/' . $post->post_media) }}">
            <a class="link-over" href="{{ url('/posts/' . $post->post_slug )}}"></a>         
            <div class="card-blog-body">              
                <small><i class="icon-clock icons mr-2"></i> {{ $post->created_at->diffForHumans() }}</small>  
                <h4 class="mt-4">{{ str_limit($post->post_title, 70) }}</h4>
                <p class="card-text mb-5">{{ str_limit($post->post_desc, 90) }}</p>
                <a class="author" href="{{ url('/profile/' . $post->user->username) }}">
                    @if (substr( $post->user->avatar, 0, 4 ) === "http")
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ $post->user->avatar }}">
                    @else
                    <img class="avatar-sm img-fluid rounded-circle" src="{{ url('/images/' . $post->user->avatar) }}">
                    @endif
                    <div>
                    {{ $post->user->name }}
                    </div>
                    @isset($post->user->role)
                        <div class="vbadge"> 
                            <img src="{{ url('/images/patch-check-fill-w.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                        </div>
                    @endisset
                </a>
                 <div class="card-like">
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
                <div class="card-count" id="likeCount{{ $post->id }}">{{ shortNumber($post->likes()->count()) }}</div>
                </div>
                </div>
            </div>
        </div>
        @endif
    @else
    <div class="card border-one text-white text-center stripes {{ $post->post_color }}">
        <div class="card-txt-body bg-over">
            <small><i class="icon-clock icons mr-2"></i> {{ $post->created_at->diffForHumans() }}</small> 
            <a class="link-over" href="{{url('/posts/' . $post->post_slug)}}"></a>
            <div class="card-like">
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
            <div class="card-count" id="likeCount{{ $post->id }}">{{ shortNumber($post->likes()->count()) }}</div>
            </div>
            @if (count($post->tags))
                <div class="category">
                    @foreach ($post->tags as $tag)
                        <a class="{{ $tag->color }}" href="{{ url('/category/' . $tag->name) }}">{{ $tag->name }}</a>
                    @endforeach
                </div>
            @endif
            <h4 class="mt-4">{{ str_limit($post->post_title, 70) }}</h4>
            <p class="card-text mb-5">{{ str_limit($post->post_desc, 90) }}</p>
            <a class="author" href="{{ url('/profile/' . $post->user->username) }}">
                @if (substr( $post->user->avatar, 0, 4 ) === "http")
                <img class="avatar-sm img-fluid rounded-circle" src="{{ $post->user->avatar }}">
                @else
                <img class="avatar-sm img-fluid rounded-circle" src="{{ url('/images/' . $post->user->avatar) }}">
                @endif
                <div>
                {{ $post->user->name }}
                </div>
                @isset($post->user->role)
                    <div class="vbadge"> 
                        <img src="{{ url('/images/patch-check-fill-w.svg') }}" alt="Verified" alt="Verified" width="14" height="14" title="Verified">
                    </div>
                @endisset
            </a>
        </div>
    </div>
    @endif
