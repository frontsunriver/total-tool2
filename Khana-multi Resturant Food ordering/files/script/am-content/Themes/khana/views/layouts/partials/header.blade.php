<header id="header">
    <div class="header-area {{ Request::is('store/*') ? 'header_fixed' : '' }}">
        <input type="hidden" id="header_close" value="{{ route('header.notify') }}">
        
        @if(!Session::has('header_notify'))
        @if(!Request::is('store*'))
        <div class="header-notify-bar">
            <div class="notify-close-btn">
                <a href="#"><span class="ti-close"></span></a>
            </div>
            <div class="notify-content text-center">
                <svg class="svg-stroke-container" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48">
                    <path class="rider-icon__path" d="M44.66,9.09,31.19,5.18a4.66,4.66,0,0,0-2.59,0L11,10.13a4.62,4.62,0,0,0-3.37,4.36s0,0,0,.07v4C4.51,21.9.1,27.21,0,31.91A10.57,10.57,0,0,0,0,33a5,5,0,0,0,2.69,4.08,5.66,5.66,0,0,0,2.64.65,13.73,13.73,0,0,0,4.27-1,7.73,7.73,0,0,0,3.7,5.55A5.41,5.41,0,0,0,16,43a17.66,17.66,0,0,0,5.93-1.66l1.16.58a4.66,4.66,0,0,0,2.07.48,4.55,4.55,0,0,0,1.26-.17l18.16-5.91A4.62,4.62,0,0,0,48,31.87V13.51A4.64,4.64,0,0,0,44.66,9.09Zm-33.35,2,9-2.52,13.18,3.83A3.62,3.62,0,0,1,36,15.86v5L26.28,24V18.37a2.56,2.56,0,0,0-1.92-2.44L11.17,12.1a2.85,2.85,0,0,0-1.45,0A3.64,3.64,0,0,1,11.31,11.11ZM4.79,36a4,4,0,0,1-2.14-3.89c.17-2.62,2.47-5.88,5-8.82V33a2.53,2.53,0,0,0,1.52,2.29l.39.18c0,.1,0,.21,0,.32C7.84,36.31,6.09,36.61,4.79,36Zm16,4.75c-2,.66-4.14,1-5.61.37a4.89,4.89,0,0,1-3-4.48v0l4.23,2,4.58,2.13Zm2-.31-6-2.78-4.58-2.13s0-.08,0-.12c.91-4.8,6.24-12.14,9.08-15.16a1,1,0,0,0-.42-1.68l-1.21-.41a1,1,0,0,0-1.07.28c-1.78,1.95-7.92,9.28-8.92,15.48,0,.14-.06.28-.07.41l0,0A1.51,1.51,0,0,1,8.69,33V22.13c1.64-1.81,3.26-3.43,4.38-4.62a1,1,0,0,0-.43-1.68l-1.2-.41a1,1,0,0,0-1.08.28c-.36.4-.95,1-1.67,1.74v-3a1.5,1.5,0,0,1,.62-1.14A1.74,1.74,0,0,1,10.37,13a1.79,1.79,0,0,1,.51.07l13.19,3.83a1.55,1.55,0,0,1,1.17,1.46V39.08a1.48,1.48,0,0,1-.72,1.26A1.84,1.84,0,0,1,22.8,40.45ZM47,31.87a3.6,3.6,0,0,1-2.7,3.46L26.11,41.24a3.25,3.25,0,0,1-1.35.1,1.41,1.41,0,0,0,.31-.14,2.49,2.49,0,0,0,1.21-2.12v-5.5L47,27.11Zm0-14.5-9.89,3.16V15.86a4.64,4.64,0,0,0-3.34-4.42L22.12,8.06l6.76-1.91a3.77,3.77,0,0,1,1-.14,3.55,3.55,0,0,1,1,.15l13.47,3.92A3.59,3.59,0,0,1,47,13.51Z"></path>
                </svg>
                <span id="rider_team_title">{{ content('header','rider_team_title') }}</span>
                <a href="{{ route('rider.register') }}" id="rider_button_title">{{ content('header','rider_button_title') }}</a>
            </div>
        </div>
        @endif
        @endif
        <div class="header-top-area">
            <div class="container">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="header-top-left-area">
                            <div class="header-language">

                                @php 
                                $get_data = App\Category::where([
                                    ['type','lang'],
                                    ['status',1]
                                ])->get();
                                @endphp
                                <input type="hidden" id="lang_url" value="{{ route('language.set') }}">
                                <select id="select_language" name="language">
                                    @foreach($get_data as $key=>$data)
                                    @php
                                        $info = json_decode($data->content);
                                    @endphp
                                    <option {{ Session::get('locale') == $data->slug ? 'selected' : ''  }} value="{{ $data->slug }}">{{ $info->lang_name }}</option>
                                    @endforeach
                                </select>
                            </div>
                            <div class="contact-number">
                                <span class="ti-mobile"></span>
                                <span id="header_pn">{{ content('header','header_pn') }}</span>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="header-top-right-section f-right">
                            @if(Auth::guest())
                            <a href="{{ url('restaurant/register') }}">{{ __('Register Your Restaurant') }}</a>
                            @endif
                            @if(Auth::guest())
                            <a href="{{ url('/user/login') }}"><span class="ti-user"></span>{{ __('Login') }}</a>
                            @endif
                            @if(Auth::check())
                            
                            <a href="{{ url('/contact') }}">{{ __('Contact Us') }}</a>
                            @endif
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="header-main-area">
            <div class="container">
                <div class="row">
                    <div class="col-lg-3">
                        <div class="header-logo">
                            <a href="{{ url('/') }}" class="pjax"><img id="logo" src="{{ asset(content('header','logo')) }}" alt="{{ env('APP_NAME') }}"></a>
                        </div>
                    </div>
                    <div class="col-lg-9">
                        <div class="header-main-right-area">
                                @if(Auth::check())
                                <div class="menu_profile">
                                    <a href="#" class="f-right" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img class="profile_img" src="{{ asset(Auth::User()->avatar) }}"></a>
                                    <div class="dropdown-menu dropdown-menu-right">
                                        <a href="{{ route('author.dashboard') }}" class="dropdown-item">{{ __('Dashboard') }}</a>
                                        <a href="{{ route('author.dashboard') }}" class="dropdown-item">{{ __('My Orders') }}</a>
                                        <a href="{{ route('author.dashboard') }}" class="dropdown-item">{{ __('Settings') }}</a>
                                        <a href="{{ route('logout') }}" onclick="event.preventDefault();
                                        document.getElementById('logout-form').submit();" class="dropdown-item">{{ __('Logout') }}</a>
                                        <form id="logout-form" action="{{ route('logout') }}" method="POST" class="d-none">
                                            @csrf
                                        </form>
                                    </div>
                                </div>
                                @endif
                                @if(Session::has('restaurant_cart'))
                                <a href="{{ route('store.show',Session::get('restaurant_cart')['slug']) }}" class="desktop_cart_icon"><div class="shopping-cart f-right">
                                    <span class="ti-shopping-cart"></span>
                                    @if(Session::has('restaurant_cart'))
                                    @if(Request::is('store/'.Session::get('restaurant_cart')['slug']) || Request::is('/') || Request::is('area*') || Request::is('checkout'))
                                    <div class="count_load">{{ Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->count() }}</div>
                                    @else
                                    <div class="count_load">0</div>
                                    @endif
                                    @else
                                    <div class="count_load">0</div>
                                    @endif
                                </div></a>
                                <a id="toggle-sidebar-left" class="mobile_cart_icon" href="#"><div class="shopping-cart f-right">
                                    <span class="ti-shopping-cart"></span>
                                    @if(Session::has('restaurant_cart'))
                                    @if(Request::is('store/'.Session::get('restaurant_cart')['slug']) || Request::is('/') || Request::is('area*') || Request::is('checkout'))
                                    <div class="count_load">{{ Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->count() }}</div>
                                    @else
                                    <div class="count_load">0</div>
                                    @endif
                                    @else
                                    <div class="count_load">0</div>
                                    @endif
                                </div></a>
                                @else
                                <a href="#" id="toggle-sidebar-left"><div class="shopping-cart f-right">
                                    <span class="ti-shopping-cart {{ !Auth::check() ? 'mr-15' : '' }}"></span>
                                    <div class="count_load"></div>
                                </div></a>
                                @endif

                                <div class="main-menu f-right">
                                    <div class="mobile-menu">
                                        <a class="toggle f-right" href="#" role="button" aria-controls="hc-nav-1"><i class="ti-menu"></i></a>
                                    </div>
                                    <nav id="main-nav">
                                        <ul>
                                         <li></li>

                                         {{ Menu('Header','submenu','','','right',true) }}
                                     </ul>
                                 </nav>


                             </div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
 </div>
 @php
 $currency=\App\Options::where('key','currency_name')->select('value')->first();
 @endphp
 <form action="{{ route('checkout.index') }}" class="cartform">
 <div class="sidebar d-none" id="sidebar-left">
  <div class="sidebar-wrapper">
    @if(Session::has('restaurant_cart'))
    @if(Session::has('cart'))
    <div class="main_cart_ok">
        <div class="delivery-main-content sidebar text-center">
            @if(Cart::instance('cart_'.Session::get('restaurant_cart')['slug'])->count() > 0)
            @php 
            $store = App\User::where('slug',Session::get('restaurant_cart')['slug'])->with('pickup','delivery')->first();
            @endphp
            <div class="delivery-toogle-action">
                <span class="delivery-title">{{ __('Delivery') }}</span>
                <div class="custom-control custom-switch">
                    <input type="checkbox" name="delivery_type" value="0" class="custom-control-input" id="hello_id"> <label class="custom-control-label" for="hello_id">{{ __('Pick Up') }}</label>
                    <input type="hidden" id="checkout_type" value="{{ route('checkout.type') }}">
                </div>
            </div>
            <input type="hidden" id="pickup_price" value="{{ $store->pickup->content }}">
            <input type="hidden" id="delivery_price" value="{{ $store->delivery->content }}">
            <div class="delivery-avg-time" id="dummy">
                <i class="fas fa-truck"></i> {{ $store->delivery->content }} {{ __('min') }}
            </div>
            <div class="delivery-order-form">
                <h5>{{ __('Your order') }} {{ $store->name }}</h5>
            </div>
            <div class="cart-product-list">
                @foreach(Cart::instance('cart_'.$store->slug)->content() as $cart)
                <div class="single-cart-product d-flex">
                    <div class="cart-product-title d-block">
                        <h5>{{ $cart->name }}</h5>
                        <p>{{ $cart->options->type }}</p>
                    </div>
                    <div class="cart-price-action d-block">
                        <span>{{ strtoupper($currency->value) }} {{ number_format($cart->price,2) }}</span>
                        <div class="cart-product-action d-flex">
                            @if($cart->qty > 1)
                            <a href="javascript:void(0)" class="right" onclick="limit_minus('{{ $cart->rowId }}','{{ $store->slug }}')"><span class="ti-minus"></span></a>
                            @else
                            <a href="javascript:void(0)" onclick="delete_cart('{{ $cart->rowId }}','{{ $store->slug }}')" class="right"><span class="fas fa-trash"></span></a>
                            @endif
                            <div class="qty">
                                <input type="text" id="total_limit{{ $cart->rowId }}" value="{{ $cart->qty }}">
                            </div>
                            <a href="javascript:void(0)" class="left" onclick="limit_plus('{{ $cart->rowId }}','{{ $store->slug }}')"><span class="ti-plus"></span></a>
                        </div>
                    </div>
                </div>
                @endforeach
            </div>
            <div class="cart-product-another-information">
                <div class="single-information d-flex">
                    <span>{{ __('Subtotal') }}</span>
                    <div class="main-amount">
                        <span>{{ strtoupper($currency->value) }} {{ Cart::subtotal() }}</span>
                    </div>
                </div>
                <div>
                    <div class="checkout-btn">
                        <a href="javascript:void(0)" onclick="$('.cartform').submit()">{{ __('Checkout') }}</a>
                    </div>
                </div>
            </div>
            @else
            <h5 class="mt-20 mb-15">{{ __('No Item in your Cart') }}</h5>
            <p class="mb-15">{{ __("You haven't added anything in your cart yet! Start adding the products you like.") }}</p>
            <div class="cart-product-another-information">
                <div class="single-information d-flex">
                    <span>{{ __('Subtotal') }}</span>
                    <div class="main-amount">
                        <span>{{ strtoupper($currency->value) }} {{ Cart::subtotal() }}</span>
                    </div>
                </div>
                <div class="checkout-btn disabled">
                    <a href="#" class="disabled">{{ __('Checkout') }}</a>
                </div>
            </div>
            @endif
        </div>
    </div>
    @else
    <div class="main_cart_ok">
        <div class="delivery-main-content sidebar text-center">
            <h5 class="mt-20 mb-15">{{ __('No Item in your Cart') }}</h5>
            <p class="mb-15">{{ __("You haven't added anything in your cart yet! Start adding the products you like.") }}</p>
            <div class="cart-product-another-information">
                <div class="single-information d-flex">
                    <span>{{ __('Subtotal') }}</span>
                    <div class="main-amount">
                        <span>{{ strtoupper($currency->value) }} {{ Cart::subtotal() }}</span>
                    </div>
                </div>
                <div class="checkout-btn disabled">
                    <a href="#" class="disabled">{{ __('Checkout') }}</a>
                </div>
            </div>
        </div> 
    </div>
    @endif
    @else
    <div class="main_cart_ok">
        <div class="delivery-main-content sidebar text-center">
            <h5 class="mt-20 mb-15">{{ __('No Item in your Cart') }}</h5>
            <p class="mb-15">{{ __("You haven't added anything in your cart yet! Start adding the products you like.") }}</p>
            <div class="cart-product-another-information">
                <div class="single-information d-flex">
                    <span>{{ __('Subtotal') }}</span>
                    <div class="main-amount">
                        <span>{{ strtoupper($currency->value) }} {{ Cart::subtotal() }}</span>
                    </div>
                </div>
                <div class="checkout-btn disabled">
                    <a href="#" class="disabled">{{ __('Checkout') }}</a>
                </div>
            </div>
        </div> 
    </div>
    @endif
</div>
</div>
</form>
<input type="hidden" id="cart_update" value="{{ route('cart.update') }}">
<input type="hidden" id="cart_delete" value="{{ route('cart.delete') }}">
</header>




