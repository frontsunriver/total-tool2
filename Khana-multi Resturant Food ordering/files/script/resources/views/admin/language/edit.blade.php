@extends('layouts.backend.app')
@section('style')
<link rel="stylesheet" href="{{ asset('admin/assets/css/selectric.css') }}">
@endsection
@section('content')
<section class="section">
    <div class="section-header">
      <h1>Customize Language</h1>
    </div>
    <div class="section-body">
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Customize Language</h6>
            </div>
            <div class="card-body">
                <div class="table_append">
                    <table class="table table-bordered">
                        <thead>
                        <tr>
                            <th scope="col">Key</th>
                            <th scope="col">Value</th>
                            <th scope="col">Action</th>
                        </tr>
                        </thead>
                        <tbody>
                            @foreach($langs as $key=>$lang)
                            <tr>
                                <td>{{ $key }}</td>
                                <td> 
                                    {{ $lang }}
                                </td>
                                <td><a href="#" class="btn btn-info"  data-toggle="modal" data-target="#lang_model_{{ Str::slug($lang) }}">Edit</a></td>
                            </tr>
                            @endforeach
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</section>
@section('extra')
@foreach($langs as $key=>$lang)
<div class="modal fade langmodel" id="lang_model_{{ Str::slug($lang) }}" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Edit Value</h5>
          <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
          </button>
        </div>
        <form action="{{ route('admin.lang.update',['lang_code'=>$id,'theme_name'=>$theme_name]) }}" method="POST" class="langform basicform">
            @csrf
            <div class="modal-body">
                <div class="form-group">
                <label for="message-text" class="col-form-label">Value:</label>
                <textarea class="form-control text-lg" name="value">{{ $lang }}</textarea>
                </div>
            </div>
            <input type="hidden" name="key" value="{{ $key }}">
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary">Update</button>
            </div>
        </form>
      </div>
    </div>
  </div>
@endforeach
@endsection
@endsection

@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
    "use strict";
    function success(res){
        $('.langmodel').modal('hide');
        $('.table_append').load(' .table_append');
    }
    function errosresponse(xhr){
        $('.alert-success').hide();
        $('.alert-danger').show();
        $('#errors').append("<li class='text-white'>"+xhr.responseJSON.message+"</li>")
        
    }
</script>
@endsection