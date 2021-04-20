@extends('theme::layouts.app')

@section('content')
 <!-- map area start -->
    <section>
        <div class="map-area">
            <div class="container-fluid p-0">
                <div class="iframe-filter-map">
                   <div id="contact-map"></div>
                </div>
            </div>
        </div>
    </section>
    <!-- map area end -->

    <!-- filter main area start -->
    <div class="filter-main-area">
        <div class="container">
            <div class="row pt-50">
                <div class="col-lg-6">
                    <div class="offer-title">
                        <h3><span id="total_resurent"></span> {{ __('Restaurants') }}</h3>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="resturant-pagination f-right">
                        <div class="d-flex">
                            <div class="left-number">
                                <span id="from"></span> - <span id="to"></span>
                            </div>
                            <div class="center-number">
                                {{ __('of') }}
                            </div>
                            <div class="right-number" >
                                <span id="total"></span>
                            </div>
                            <div class="left-icon">
                                <a href="javascript:void(0)" id="last_page_url"><i class="fas fa-angle-left"></i></a>
                            </div>
                            <div class="right-icon">
                                <a href="javascript:void(0)" id="next_page_url"><i class="fas fa-angle-right"></i></a>
                            </div>
                        </div>
                    </div>

                    <div class="offer-dropdown f-right">
                        <div class="form-group">
                            <select class="form-control" id="order">
                                <option value="DESC">{{ __('Latest') }}</option>
                                <option value="ASC">{{ __('Old') }}</option>
                                
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-3">
                    <div class="filter-left-section">
                        <div class="single-filter last">
                            <div class="filter-main-title">
                                <h4>{{ __('Filter By') }}</h4>
                            </div>
                        </div>
                        <div class="single-filter search-area">
                            <div class="filter-search-area">
                                <div class="form-group">
                                    <input type="text" id="restaurants_search" class="form-control" placeholder="{{ __('button_title') }}">
                                </div>
                            </div>
                        </div>
                        <div class="single-filter">
                            <div class="single-filter-title">
                                <span>{{ __('Select Location') }}</span>
                                <div class="sidebar-body">
                                    <div class="category-list">
                                        <nav>
                                            <ul>

                                               @php
                                                $locations=\App\Terms::where('type',2)->where('status',1)->get();
                                                $crntid=$info->id ?? 0;
                                               @endphp

                                               @foreach($locations as $key=> $row)
                                                <li>
                                                    <div class="custom-control custom-checkbox">
                                                        <input @if($crntid ==$row->id) checked @endif type="radio" class="custom-control-input area" id="customCheck{{ $key }}" value="{{ $row->id }}" name="area">
                                                        <label class="custom-control-label" for="customCheck{{ $key }}">{{ $row->title }}</label>
                                                    </div>
                                                </li>
                                                @endforeach
                                            </ul>
                                        </nav>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="single-filter">
                            <div class="single-filter-title">
                                <span>{{ __('category_title') }}</span>
                                <div class="sidebar-body">
                                    <div class="category-list scroll">
                                        <nav>
                                            <ul>
                                                 
                                               @php
                                               $categories=\App\Category::where('type',2)->select('id','name')->latest()->get();
                                               if (isset($category)) {
                                                   $cat_row=$category->id;
                                               }
                                               else{
                                                $cat_row='';
                                               }
                                               @endphp
                                               @foreach($categories as $key => $row)
                                               <li>
                                                    <div class="custom-control custom-checkbox">
                                                        <input  type="radio" class="custom-control-input cat" id="customCheckaa{{ $key }}" value="{{ $row->id }}" @if($cat_row == $row->id) checked @endif name="gfg">
                                                        <label class="custom-control-label" for="customCheckaa{{ $key }}">{{ $row->name }}</label>
                                                    </div>
                                                </li>
                                                @endforeach
                                                
                                            </ul>
                                        </nav>
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                    </div>
                </div>
                <div class="col-lg-9">
                    <div class="filter-right-section">
                        <div class="row" id="resturent_area">
                            <div class="loader-main-area d-none"><div class="loader-area"><div class="loader"></div></div></div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- filter main area end -->
    
    <input type="hidden" id="location_slug" value="{{ $slug ?? '' }}">
    <input type="hidden" id="baseurl" value="{{ url('/') }}">
    <input type="hidden" id="location_id" value="{{ $info->id ?? 00 }}">
   

@endsection

@push('js')
<script src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}"></script> 
<script type="text/javascript">
    "use strict";
    var current_lat= {{ $lat }};
    var current_long= {{ $long }};
    var current_zoom= {{ $zoom }};
    var default_image= '{{ asset('uploads/store.jpg') }}';
    var resturent_icon= '{{ asset('uploads/location.png') }}';
</script>
<script src="{{ theme_asset('khana/public/js/area.js') }}"></script>

@endpush