@extends('theme::layouts.app')
@section('content')
<!-- hero area start -->
<section id="hero">
    <div class="hero-area">
        <div class="container">
            <div class="row">
                <div class="col-lg-8">
                    <div class="hero-main-content">
                        <h2 id="hero_title">{{ content('hero','hero_title') }}</h2>
                        <p id="hero_des">{{ content('hero','hero_des') }}</p>
                        <div class="food-search-bar">
                            <form action="{{ route('resturents.search') }}" id="searchform">
                                <div class="search-input">
                                    <div class="row w-100">
                                        <div class="col-lg-9 pr-0">
                                            <input type="hidden" id="lat" name="lat" required="">
                                            <input type="hidden" id="long" name="long" required="">
                                            <input type="hidden" id="city" name="city" required="">
                                            <div class="slider-search-content-area">
                                                <input type="text" placeholder="{{ __('Enter your delivery location') }}" id="location_input" name="address" required="">
                                                <div class="location-icon" id="locationIcon"><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" id="ic-locate-me-round" width="24" height="24"><defs><path id="a" d="M11.5 18a5.5 5.5 0 1 0 0-11 5.5 5.5 0 0 0 0 11zM12 4v2.019A6.501 6.501 0 0 1 17.981 12H20v1h-2.019a6.501 6.501 0 0 1-5.98 5.981L12 21h-1v-2.019a6.501 6.501 0 0 1-5.981-5.98L3 13v-1h2.019A6.501 6.501 0 0 1 11 6.02V4h1zm-.5 11a2.5 2.5 0 1 0 0-5 2.5 2.5 0 0 0 0 5zm0 1a3.5 3.5 0 1 1 0-7 3.5 3.5 0 0 1 0 7zm0-2.5a1 1 0 1 0 0-2 1 1 0 0 0 0 2z"></path></defs><use fill="{{ $theme_color }}" xlink:href="#a"></use></svg></div>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 pr-0">
                                            <button type="submit" id="button_title">{{ content('hero','button_title') }}</button>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                        <div class="hero-main-categories-list">
                            <p id="offer_title">{{ content('hero','offer_title') }}</p>
                            <div class="owl-carousel recent_sell_restaurants">
                                @foreach($offer_resturents as $resturent)

                                <a href="{{ route('store.show',$resturent->user->slug) }}">
                                <div class="hero-category">
                                    <div class="hero-single-category text-center">
                                        <img class="lazy" src="{{ asset('uploads/lazyload-40x40.png') }}" data-src="{{ asset(imagesize($resturent->userwithpreview->preview->content,'small')) }}" alt="{{ $resturent->user->title }}">
                                        <p>{{ Str::limit($resturent->userwithpreview->name,8) }}</p>
                                        <span class="text-dark">{{ $resturent->count }}{{ __('% OFF') }}</span>
                                    </div>
                                </div>
                                </a>
                                @endforeach
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="slider-img">
                        <img id="hero_right_image"  class="lazy" src="{{ asset('uploads/lazyload-138x135.png') }}" data-src="{{ asset(content('hero','hero_right_image')) }}" alt="{{ env('APP_NAME') }}">
                        <div class="slider-content">
                            <h3 id="hero_right_title">{{ content('hero','hero_right_title') }}</h3>
                            <h5 id="hero_right_content">{{ content('hero','hero_right_content') }}</h5>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<!-- hero area end -->
@php
$categories=\App\Category::where('type',2)->select('name','slug','avatar')->inRandomOrder()->take(20)->get();

@endphp
@if(count($categories) > 0)
<!-- all restaurants slider area start -->
<div class="restaurants-slider-area pt-50 pb-50" id="category">
    <div class="container">
        <div class="row">
            <div class="col-lg-12">
                <div class="restaurants-slider-title">
                    <h3 id="category_title">{{ content('category','category_title') }}</h3>
                </div>
            </div>
            
        </div>
        <div class="owl-carousel restaurants">

            
            @foreach($categories as $key => $row)
            <div class="col-lg-12 p-0">
                <div class="single-restaurants">
                    <a href="{{ route('category',$row->slug) }}">
                        <img class="lazy" src="{{ asset('uploads/lazyload-138x135.png') }}" data-src="{{ asset(imagesize($row->avatar,'medium')) }}" alt="{{ $row->name }}">
                        <h5>{{ $row->name }}</h5>
                    </a>
                </div>
            </div>
            @endforeach
           
        </div>
    </div>
</div>
<!-- all restaurants slider area end -->
@endif
<!-- best rated restaurants area start -->
@if(count($top_rated) > 0)
<div class="best-rated-restaturants pt-50" id="best_restaurant">
    <div class="container">
        <div class="row mb-20">
            <div class="col-lg-12">
                <h3 id="best_restaurant_title">{{ content('best_restaurant','best_restaurant_title') }}</h3>
            </div>
        </div>
        <div class="row" id="best_rated">
             @foreach($top_rated as $row)

            <div class="col-lg-3 mb-30">
                <a href="{{ route('store.show',$row['slug']) }}">
                    <div class="single-restaturants">
                        <img class="lazy" src="{{ asset('uploads/lazyload-250x186.png') }}" data-src="{{  asset($row['avatar']) }}" alt="{{ $row['name'] }}">
                        @if(!empty($row['coupon']))
                        <div class="badges">
                           
                            <span>{{ __('COUPON:') }} {{ $row['coupon']->title }}</span>
                            <span>{{ $row['coupon']->count }}% {{ __('off') }}</span>
                        </div>
                        @endif
                        <div class="total-min">
                            <span>{{ $row['time'] }}</span><span>{{ __('min') }}</span>
                        </div>
                        <div class="restaturants-content">
                            <div class="name-rating">
                                <h4>{{ Str::limit($row['name'],15) }}</h4>
                                <span class="ratings-component"><span class="stars"><svg xmlns="http://www.w3.org/2000/svg" width="12" height="11"><path fill="{{ $theme_color }}" d="M9 7.02L9.7 11 6 9.12 2.3 11 3 7.02 0 4.2l4.14-.58L6 0l1.86 3.62L12 4.2z"></path></svg></span>
                                <span class="rating"><b>{{ $row['avg_ratting'] }}</b>/
                                5</span><span class="count">({{ $row['rattings'] ?? 0 }})</span></span>
                            </div>
                            <div class="category-summery">

                                <nav>
                                    <ul>
                                       <li><a class="text-dark" href="{{ route('area',$row['city']['slug']) }}">{{ $row['city']['title'] }}</a>
                                       </li>
                                       
                                    </ul>
                                </nav>
                            </div>
                            
                        </div>
                    </div>
                </a>
            </div>
           @endforeach
        </div>
    </div>
</div>
@endif

@if(count($locations) > 0)
<!-- all place area start -->
<div class="all-place-area pt-70" id="city_area">
    <div class="container">
        <div class="row pb-30">
            <div class="col-lg-12">
                <h3 id="find_city_title">{{ content('city_area','find_city_title') }}</h3>
            </div>
        </div>
        <div class="row">
            @foreach($locations as $key=> $row)
             <div class="col-lg-3 mb-30">
                <div class="single-place">
                    <a href="{{ route('area',$row['slug']) }}">
                        <img class="lazy" src="{{ asset('uploads/lazyload-250x186.png') }}" data-src="{{ asset($row['preview']) }}" alt="{{ $row['title'] }}">
                        <div class="single-place-content">
                            <h1>{{ Str::limit($row['title'],1,' ') }}</h1>
                            <div class="name_city_content d-flex">
                                 <h4>{{ $row['title'] }}</h4>
                                <a class="ml-auto" href="{{ route('area',$row['slug']) }}"><svg class="svg-stroke-container"
                                        xmlns="http://www.w3.org/2000/svg" width="20" height="18"
                                        viewBox="0 0 20 18">
                                        <g fill="none" fill-rule="evenodd" stroke="#FFFFFF" stroke-width="2"
                                            transform="translate(1 1)" stroke-linecap="round">
                                            <path d="M0,8 L17.5,8"></path>
                                            <polyline points="4.338 13.628 15.628 13.628 15.628 2.338"
                                                stroke-linejoin="round" transform="rotate(-45 9.983 7.983)">
                                            </polyline>
                                        </g>
                                    </svg>
                                </a>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
            @endforeach
        </div>
    </div>
</div>
<!-- all place area end -->
@endif
<!-- best rated restaurants area end -->
@if(count($features_resturent) > 0)
<!-- Fast delivery restaurants area start -->
<div class="best-rated-restaturants pt-70" id="featured_resturent">
    <div class="container">
        <div class="row mb-20">
            <div class="col-lg-12">
                <h3 id="featured_resturent_title">{{ content('featured_resturent','featured_resturent_title') }}</h3>
            </div>
        </div>
        <div class="row">
            @foreach($features_resturent as $row)

            <div class="col-lg-3 mb-30">
                <a href="{{ route('store.show',$row['slug']) }}">
                    <div class="single-restaturants">
                        <img class="lazy" src="{{ asset('uploads/lazyload-250x186.png') }}" data-src="{{  asset($row['avatar']) }}" alt="{{ $row['name'] }}">
                        @if(!empty($row['coupon']))
                        <div class="badges">
                           <span>{{ __('COUPON:') }} {{ $row['coupon']->title }}</span>
                            <span>{{ $row['coupon']->count }}% {{ __('off') }}</span>
                        </div>
                        @endif
                        <div class="total-min">
                            <span>{{ $row['time'] }}</span><span>{{ __('min') }}</span>
                        </div>
                        <div class="restaturants-content">
                            <div class="name-rating">
                                <h4>{{ Str::limit($row['name'],15) }}</h4>
                                <span class="ratings-component"><span class="stars"><svg xmlns="http://www.w3.org/2000/svg" width="12" height="11"><path fill="#FF3252" d="M9 7.02L9.7 11 6 9.12 2.3 11 3 7.02 0 4.2l4.14-.58L6 0l1.86 3.62L12 4.2z"></path></svg></span>
                                <span class="rating"><b>{{ $row['avg_ratting'] }}</b>/
                                5</span><span class="count">({{ $row['rattings'] ?? 0 }})</span></span>
                            </div>
                            <div class="category-summery">
                                  <nav>
                                    <ul>
                                        <li><a class="text-dark" href="{{ route('area',$row['city']['slug']) }}">{{ $row['city']['title'] }}</a>
                                       </li>
                                       
                                    </ul>
                                </nav>
                            </div>
                            
                        </div>
                    </div>
                </a>
            </div>
           @endforeach
           
            
        </div>
    </div>
</div>
<!-- Fast delivery restaurants area end -->
@endif


@endsection

@push('js')
<script defer src="https://maps.googleapis.com/maps/api/js?libraries=places&language=en&key={{ env('PLACE_KEY') }}"></script>
<script src="{{ theme_asset('khana/public/js/store/home.js') }}"></script>
<script src="{{ theme_asset('khana/public/js/jquery.unveil.js') }}"></script>
        
@endpush