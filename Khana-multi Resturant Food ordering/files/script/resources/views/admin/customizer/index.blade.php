<!doctype html>
<html class="no-js" lang="">

<head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>LPress | Customizer</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="shortcut icon" type="image/x-icon" href="img/favicon.ico">
    <!-- Place favicon.ico in the root directory -->

    <!-- CSS here -->
    <link rel="stylesheet" href="{{ asset('customizer/css/bootstrap.min.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/css/fontawesome-all.min.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/fonts/flaticon/flaticon.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/css/font.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/css/default.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/css/style.css') }}">
    <link rel="stylesheet" href="{{ asset('customizer/css/responsive.css') }}">
</head>

<body class="customizer_loader">
    <!--[if lte IE 9]>
            <p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="https://browsehappy.com/">upgrade your browser</a> to improve your experience and security.</p>
        <![endif]-->
        <div class="loader">
            <div class="fusion-slider-loading">
            </div>
        </div>  

        <!-- customizer main area start -->
        <div class="customizer-main-area">
            <div class="container-fluid pl-0">
                <div class="row">
                    <div class="col-lg-3 sidebar">
                        <div class="sidebar-area-start">
                           <div class="loading">
                            <div class="fusion-slider-loading">
                            </div>
                        </div>
                        <div class="header-top-info d-flex">
                            <div class="logo">
                              <a href="https://codecanyon.net/user/amcoders/portfolio" target="_blank">  <img src="{{ asset('customizer/img/logo/logo.png') }}" alt=""></a>
                            </div>
                           <span>{{ __('LPress') }}</span>
                        </div>
                        <div class="main-sidebar-area">
                            <div class="sidebar_option_area">
                                @foreach($sidebar_options as $section)
                                <a href="#">
                                    <div class="single-sidebar-area" onclick="section('{{ $section['id'] }}')">
                                        <div class="sidebar-left-section">
                                            <div class="sidebar-icon">
                                                <span class="{{ $section['icon'] }}"></span>
                                            </div>
                                            <div class="sidebar-menu-name">
                                                <span>{{ $section['name'] }}</span>
                                            </div>
                                        </div>
                                        <div class="sidebar-right-section">
                                            <div class="sidebat-right-section">
                                                <span class="flaticon-squares"></span>
                                            </div>
                                        </div>
                                    </div>
                                </a>
                                @endforeach
                                <input type="hidden" id="section_url" value="{{ route('admin.customizer.section_option') }}">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-9 navbar_section">
                    <!-- navbar area start -->
                    <div class="navbar-area-start">
                        <div class="row">
                            <div class="col-lg-4">
                                <div class="navbar-page-list">
                                    <div class="page_navbar">
                                        <div class="form-group">
                                            <select class="form-control" id="multiple_page">
                                             @foreach($active_theme as $page)
                                             <option {{ $page['status'] == 'active' ? 'selected' : '' }}>{{ $page['page_name'] }}</option>
                                             @endforeach
                                         </select>
                                         <input type="hidden" id="page_change_url" value="{{ route('admin.customizer.page_change') }}">
                                     </div>
                                 </div>
                             </div>
                         </div>
                         <div class="col-lg-4">
                            <div class="display-view text-center">
                                <a href="#" id="mobile_device"><i class="fas fa-mobile-alt"></i></a>
                                <a href="#" class="active" id="labtop_device"><i class="fas fa-desktop"></i></a>
                                <a href="#" id="tablet_device"><i class="fas fa-tablet-alt"></i></a>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="navbar-live-aleart text-right">
                                @foreach($active_theme as $theme)
                                @if($theme['status'] == 'active')
                                <a href="{{ asset($theme['page_url']) }}" target="_blank" class="site-view"> <i class="flaticon-visibility view-site"></i></a>
                                @endif
                                @endforeach
                                <span>{{ __('Live') }}</span>
                                <div class="save-btn text-right">
                                    <a href="#" id="save_btn" class="btn disabled">Save</a>
                                    <input type="hidden" id="save_btn_url" value="{{ route('admin.customizer.save') }}">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- navbar area end -->
                <!-- website append area start -->
                <div class="website-append">
                    @foreach($active_theme as $theme)
                    @if($theme['status'] == 'active')
                    <iframe id="myFrame" src="{{ asset($theme['page_url']) }}" frameborder="0"></iframe>
                    @endif
                    @endforeach
                </div>
                <!-- website append area end -->
            </div>
        </div>
    </div>
</div>
<!-- customizer main area end -->


<!-- JS here -->
<script src="{{ asset('customizer/js/vendor/jquery-3.5.1.min.js') }}"></script>
<script src="{{ asset('customizer/js/popper.min.js') }}"></script>
<script src="{{ asset('customizer/js/bootstrap.min.js') }}"></script>
<script src="{{ asset('customizer/js/jquery.nicescroll.min.js') }}"></script>
<script src="{{ asset('admin/js/sweetalert2.all.min.js') }}"></script>
<script src="{{ asset('admin/js/customizer.js') }}"></script>
</body>

</html>