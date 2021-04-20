<!doctype html>
<html class="no-js" lang="">
<head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>{{ __('Installer') }}</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    
    <link rel="shortcut icon" type="image/x-icon" href="{{ asset('uploads/favicon.ico') }}">
    <!-- Place favicon.ico in the root directory -->

    <!-- CSS here -->
    <link rel="stylesheet" href="{{ asset('admin/css/bootstrap.min.css') }}">
    <link rel="stylesheet" href="{{ asset('admin/css/fontawesome-all.min.css') }}">
    <link rel="stylesheet" href="{{ asset('admin/css/font.css') }}">
    <link rel="stylesheet" href="{{ asset('admin/css/default.css') }}">
    <link rel="stylesheet" href="{{ asset('admin/css/style.css') }}">
</head>
<body class="install">
        <!--[if lte IE 9]>
            <p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="https://browsehappy.com/">upgrade your browser</a> to improve your experience and security.</p>
        <![endif]-->

    <!-- loader seaction start -->
    <section class="loading_bar">
        <div class="load">
            <div class="fusion-slider-loading">
            </div>
            <div>
                <h3 class="install-info"></h3>
                <div class="back-btn d-flex justify-content-center">
                    <a class="back btn d-none" href="{{ route('install.info') }}">Try Again</a>
                </div>
            </div>
        </div>
    </section>
    <!-- loader seaction start -->

    <!-- requirments-section-start -->
    <section class="pt-50 pb-50">
        <div class="requirments-section">
            <div class="content-requirments d-flex justify-content-center">
                <div class="requirments-main-content">
                    <div class="installer-header text-center">
                        <h2>{{ __('Configuration') }}</h2>
                        <p>{{ __('Please enter your database connection details') }}</p>
                    </div>
                 <div class="alert alert-success d-none" role="alert">
                     
                 </div>
                 <form action="{{ url('install/store') }}" method="POST" id="install">
                    @csrf
                    <div class="custom-form install-form">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="app_name">{{ __('App Name') }}</label>
                                    <input type="text" class="form-control" id="app_name" name="app_name" required placeholder="App Name without space">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label for="type">{{ __('Installation Type') }}</label>
                                    <select class="form-control" name="type" id="type">
                                        <option value="install" style="background-color: black;color: #fff">{{ __('I Want To Install') }}</option>
                                        {{-- <option value="update" style="background-color: black;color: #fff">I Want To Update</option> --}}
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="db_host">{{ __('Database Host') }}</label>
                                    <input type="text" value="localhost" class="form-control" id="db_host" name="db_host" required placeholder="Database Host">
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="db_name">{{ __('Database Name') }}</label>
                                    <input type="text" class="form-control" id="db_name" name="db_name" required placeholder="Database Name">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="db_user">{{ __('Database Username') }}</label>
                                    <input type="text" class="form-control" id="db_user" name="db_user" required placeholder="Database Username">
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label for="db_pass">{{ __('Database Password') }}</label>
                                    <input type="text" class="form-control" id="db_pass" name="db_pass" placeholder="Database Password">
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <input type="hidden" value="{{ url('/') }}" class="form-control none" id="app_url" name="app_url" required placeholder="App Url">
                                    <input type="hidden" id="migrate_url" value="{{ route('install.migrate') }}">
                                    <input type="hidden" id="seed_url" value="{{ route('install.seed') }}">
                                    <input type="hidden" id="check_url" value="{{ route('install.check') }}">
                                    <input type="hidden" id="home_url" value="{{ route('welcome') }}">
                                </div>
                            </div>
                        </div>
                    </div>
                    <button type="submit" class="btn btn-primary install-btn f-right">{{ __('Save & Install') }}</button>
                </form>
                </div>
            </div>
        </div>
    </section>
    <!-- requirments-section-end -->
    <script src="{{ asset('admin/js/vendor/jquery-3.5.1.min.js') }}"></script>
    <script src="{{ asset('admin/js/install/install.js') }}"></script>
</body>
</html>
