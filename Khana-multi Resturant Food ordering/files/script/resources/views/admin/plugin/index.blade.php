@extends('layouts.backend.app')

@section('content')
<div class="card"  >
    <div class="card-body">
        <div class="row mb-30">
            <div class="col-lg-6">
                <h4>{{ __('Plugin List') }}</h4>
            </div>
            <div class="col-lg-6">
                <div class="add-new-btn">
                    
                </div>
            </div>
        </div>
        <div class="table-responsive custom-table">
            <table class="table">
                <thead>
                    <tr>
                        <th class="am-title">{{ __('Plugin') }}</th>
                        <th class="am-title">{{ __('Description') }}</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach($plugins as $plugin)
                    <tr>
                        <td>{{ $plugin['Plugin Name'] }}
                            <div class="hover">
                                @if($plugin['status'] == 'deactive')
                                <a class="last" href="{{ route('admin.plugin.active',$plugin['Text Domain']) }}">Active</a>
                                @else
                                <a class="last" href="{{ route('admin.plugin.deactive',$plugin['Text Domain']) }}">Deactive</a>
                                @endif
                            </div>
                        </td>
                        <td>
                            <div class="plugin-des">
                                <p>{{ $plugin['Description'] }}</p>
                            </div>
                            <div class="plugin-anthor-info d-flex">
                                <span><strong>Version:</strong>{{ $plugin['Version'] }}</span>
                                <span class="last"><strong>By</strong><a target="_blank" href="{{ $plugin['Author URI'] }}">{{ $plugin['Author'] }}</a></span>
                            </div>
                        </td>
                    </tr>
                    @endforeach
                </tbody>
                <tfoot>
                    <tr>
                        <th class="am-title">{{ __('Plugin') }}</th>
                        <th class="am-title">{{ __('Description') }}</th>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>
</div>

<div class="modal fade" id="new_plugin" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalCenterTitle">{{ __('Upload Plugin') }}</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <form action="{{ route('admin.plugin.upload') }}" method="POST" enctype="multipart/form-data">
        @csrf
        <div class="modal-body mt-35 mb-35">
           <div class="custom-form">
            <div class="custom-file">
              <input type="file" class="custom-file-input" name="file" id="theme_file">
              <label class="custom-file-label" for="theme_file">{{ __('Choose file') }}</label>
          </div>
      </div>
  </div>
  <div class="modal-footer">
    <button type="submit" class="btn">{{ __('Install Now') }}</button>
</div>
</form>
</div>
</div>
</div>
@endsection