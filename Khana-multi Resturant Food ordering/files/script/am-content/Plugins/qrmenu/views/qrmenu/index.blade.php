@extends('layouts.backend.app')

@section('style')
    <link rel="stylesheet" href="{{ plugin_asset('qrmenu/public/css/style.css') }}">
@endsection

@section('content')
<section>
    <div class="section">
        <div class="section-header">
            <h4>{{ __('Restaurant QR Generators') }}</h4>
        </div>
        <div class="section-body">
            <div class="row">
                <div class="col-lg-4">
                    <div class="card">
                        <div class="card-body">
                            <div class="qr-style-area">
                                <h5>Generate QR Style</h5>
                                <hr>
                                <div class="row">
                                    <div class="col-lg-6 mb-30">
                                        <div class="single-qr-style" onclick="qr_style('square')">
                                            <img class="eye img-fluid" src="{{ plugin_asset('qrmenu/public/qr/4.png') }}" alt="">
                                        </div>
                                    </div>
                                    <div class="col-lg-6 mb-30">
                                        <div class="single-qr-style" onclick="qr_style('dot')">
                                            <img class="img-fluid" src="{{ plugin_asset('qrmenu/public/qr/3.png') }}" alt="">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="select-background-color">
                                <div class="form-group">
                                    <h5>Background Color</h5>
                                    <hr>
                                    @php
                                    if(isset($bgcolor))
                                    {
                                        $rgb = $bgcolor;
                                        $rgbarr = explode(",",$rgb,3);
                                    }
                                    @endphp 
                                    <input type="color" class="form-control"  onchange="qr_bgcolor(event)" value="{{ isset($bgcolor) ? sprintf("#%02x%02x%02x", $rgbarr[0], $rgbarr[1], $rgbarr[2]) : '' }}">
                                </div>
                            </div>
                            <div class="select-background-color">
                                <div class="form-group">
                                    <h5>Color</h5>
                                    <hr>
                                    @php
                                    if(isset($color))
                                    {
                                        $rgb = $color;
                                        $rgbarr1 = explode(",",$rgb,3);
                                    }
                                    @endphp 
                                    <input type="color" class="form-control" onchange="qr_color(event)" value="{{ isset($color) ? sprintf("#%02x%02x%02x", $rgbarr1[0], $rgbarr1[1], $rgbarr1[2]) : '' }}">
                                </div>
                            </div>
                            <div class="select-background-color">
                                <div class="form-group">
                                    <h5>Size</h5>
                                    <hr>
                                    <input type="range" min="10" max="280" class="form-control" id="qr_size" onchange="qr_size()" value="{{ $size }}">
                                </div>
                            </div>
                            <div class="select-background-color">
                                <div class="form-group">
                                    <h5>Margin</h5>
                                    <hr>
                                    <input type="range" min="1" max="100" class="form-control" id="qr_margin" onchange="qr_margin()" value="{{ $margin }}">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="card">
                        <div class="qr-code-fixed-area">
                            <div class="card-body">
                                <h5>QR Code</h5>
                                    <hr>
                                <div class="qr-code-main">
                                    {{ $qrcodegenerate }}
                                </div>
                                <div class="btn-download mt-3">
                                    <button class="btn btn-primary btn-lg w-100" onclick="downloadPng()">Download</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="card">
                        <div class="card-body">
                            <h5>Download Templates</h5>
                            <hr>
                            <div id="carouselExampleIndicators3" class="carousel slide" data-ride="carousel">
                                <ol class="carousel-indicators">
                                  <li data-target="#carouselExampleIndicators3" data-slide-to="0" class="active"></li>
                                  <li data-target="#carouselExampleIndicators3" data-slide-to="1"></li>
                                  <li data-target="#carouselExampleIndicators3" data-slide-to="2"></li>
                                </ol>
                                <div class="carousel-inner">
                                  <div class="carousel-item active">
                                    <div class="single-template">
                                        <img class="img-fluid d-block w-100" src="{{ plugin_asset('qrmenu/public/img/template/1.png') }}" alt="">
                                    </div>
                                  </div>
                                  <div class="carousel-item">
                                    <div class="single-template">
                                        <img class="img-fluid d-block w-100" src="{{ plugin_asset('qrmenu/public/img/template/2.png') }}" alt="">
                                    </div>
                                  </div>
                                  <div class="carousel-item">
                                    <div class="single-template">
                                        <img class="img-fluid d-block w-100" src="{{ plugin_asset('qrmenu/public/img/template/3.png') }}" alt="">
                                    </div>
                                  </div>
                                </div>
                                <a class="carousel-control-prev" href="#carouselExampleIndicators3" role="button" data-slide="prev">
                                  <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                  <span class="sr-only">Previous</span>
                                </a>
                                <a class="carousel-control-next" href="#carouselExampleIndicators3" role="button" data-slide="next">
                                  <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                  <span class="sr-only">Next</span>
                                </a>
                            </div>
                            <div class="template-download mt-3">
                                <a href="https://github.com/laramaster/khana-qr-psd/archive/master.zip" class="btn btn-primary w-100 btn-lg">Download Templates</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<input type="hidden" value="{{ route('store.qrmenu.style') }}" id="qrmenu_style">
@endsection

@section('script')
<script src="{{ plugin_asset('qrmenu/public/js/script.js') }}"></script>
@endsection