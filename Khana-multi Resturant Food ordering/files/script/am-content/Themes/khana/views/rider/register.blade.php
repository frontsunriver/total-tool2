<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, shrink-to-fit=no" name="viewport">
  <title>{{ __('Register') }}</title>

  <!-- General CSS Files -->
  <link rel="stylesheet" href="{{ asset('admin/assets/css/bootstrap.min.css') }}">

  <!-- Template CSS -->
  <link rel="stylesheet" href="{{ asset('admin/assets/css/style.css') }}">
  
</head>

<body>
  <div id="app">
    <section class="section">
      <div class="container mt-5">
        <div class="row">
          <div class="col-12 col-sm-10 offset-sm-1 col-md-8 offset-md-2 col-lg-8 offset-lg-2 col-xl-8 offset-xl-2">
           

            <div class="card card-primary">
              <div class="card-header"><h4>{{ __('Register') }}</h4></div>
              <div class="card-body">
               @if(Session::has('errors'))
               <div class="row">
                <div class="col-12">
                  <p class="alert alert-danger">{{ Session::get('errors') }}</p>
                </div>
              </div>
              @endif
              <form action="{{ route('rider.register') }}" method="POST">
                @csrf
                <div class="row">
                  <div class="col-6">
                    @php
                    $arr['title']=__('Full Name');
                    $arr['name']='name';
                    $arr['type']='text';
                    $arr['id']='name';
                    $arr['placeholder']=__('name');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                    
                  </div>
                  <div class="col-6">
                     @php
                    $arr['title']= __('Email');
                    $arr['name']='email';
                    $arr['type']='email';
                    $arr['id']='email';
                    $arr['placeholder']= __('Email');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                   
                  </div>
                </div>
                <div class="row">
                  <div class="col-6">
                     @php
                    $arr['title']=__('Enter Password');
                    $arr['name']='password';
                    $arr['type']='password';
                    $arr['id']='password';
                    $arr['placeholder']=__('Password');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                  </div>
                  <div class="col-6">
                     @php
                    $arr['title']=__('Password Confirmation');
                    $arr['name']='password_confirmation';
                    $arr['type']='password';
                    $arr['id']='password2';
                    $arr['placeholder']= __('Password');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                  </div>
                </div>
                
                <div class="row">
                  <div class="col-6">
                      @php
                    $arr['title']=__('Contact Number 1');
                    $arr['name']='phone1';
                    $arr['type']='text';
                    $arr['id']='phone1';
                    $arr['placeholder']=__('Contact Number 1');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                  </div>
                    
                  <div class="form-group col-6">
                     @php
                    $arr['title']=__('Contact Number 2');
                    $arr['name']='phone2';
                    $arr['type']='text';
                    $arr['id']='phone2';
                    $arr['placeholder']=__('Contact Number 2');
                    $arr['is_required']=true;
                    echo input($arr);
                    @endphp
                  </div>
               
                 </div>
                 <div class="form-divider">
                  {{ __('Your Address') }}
                </div>
                <div class="row">
                  <div class="form-group col-12">
                    <label>{{ __('Select Your City') }}</label>
                    <select class="form-control selectric" name="city">
                      @foreach($cities as $city)
                      <option value="{{ $city->id }}">{{ $city->title }}</option>
                      @endforeach
                    </select>
                  </div>
                  <div class="form-group col-12">
                    <label for="address">{{ __('Enter Full Address') }}</label>
                    <input id="location_input" type="text" class="form-control" name="full_address" autocomplete="off" placeholder="{{ __('Enter Full Address') }}">
                    <div class="invalid-feedback">
                    </div>
                  </div>
                </div>
                <input type="hidden" name="latitude" id="latitude" value="00.00">
                <input type="hidden" name="longitude" id="longitude" value="00.00">
                <div class="map-canvas-register none" id="map-canvas" ></div>
                <div class="form-group">
                  <div class="custom-control custom-checkbox">
                    <input type="checkbox" name="agree" class="custom-control-input" id="agree" required>
                    <label class="custom-control-label" for="agree">{{ __('I agree with the') }} <a href="{{ url('/page/terms-and-conditions') }}">{{ __('Terms and conditions') }}</a></label>
                  </div>
                </div>
                
                <div class="form-group">
                  <button type="submit" class="btn btn-primary btn-lg btn-block">
                    {{ __('Register') }}
                  </button>
                </div>
              </form>
            </div>
          </div>
          <div class="simple-footer">
            {{ __('Copyright') }} &copy; {{ config('app.name') }} {{ date('Y') }}
          </div>
        </div>
      </div>
    </div>
  </section>
</div>

<!-- General JS Scripts -->
<script src="{{ asset('admin/assets/js/jquery-3.5.1.min.js') }}"></script>
<script src="{{ asset('admin/assets/js/popper.min.js') }}"></script>
<script src="{{ asset('admin/assets/js/bootstrap.min.js') }}"></script>
<script async defer src="https://maps.googleapis.com/maps/api/js?key={{ env('PLACE_KEY') }}&libraries=places&callback=initialize"></script>
<script src="{{ asset('admin/assets/js/rideregister.js') }}"></script>
</body>
</html>
