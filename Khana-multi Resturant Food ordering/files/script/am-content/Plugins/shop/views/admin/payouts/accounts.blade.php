@extends('layouts.backend.app')
@section('content')
@include('layouts.backend.partials.headersection',['title'=>'Payout Accounts'])
<div class="row">
  <div class="col-12 mt-2">
    <div class="card">
      <div class="card-body">
      <div class="float-right">
        <form>
          <div class="input-group mb-2 col-12">

            <input type="number" class="form-control" placeholder="User id" required="" name="src" autocomplete="off" value="{{ $src ?? '' }}" >
            <select class="form-control" name="type">
              
              <option value="paypal_info">{{ __('Search By User ID With Paypal') }}</option>
              <option value="bank_info">{{ __('Search By User ID With Bank') }}</option>
            </select>
            <div class="input-group-append">                                            
              <button class="btn btn-primary" type="submit"><i class="fas fa-search"></i></button>
            </div>
          </div>
        </form>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-hover text-center table-borderless">
          <thead>
            <tr>
              <th>{{ __('User Id') }}</th>
              <th>{{ __('Method Name') }}</th>                      
              <th>{{ __('Created At') }}</th>
              <th>{{ __('Action') }}</th>
            </tr>
          </thead>
          <tbody>
            @foreach($accounts as $row)
            <tr>
              <td><a href="{{ route('admin.vendor.show',$row->user_id) }}">#{{ $row->user_id }}</a></td>
              <td>{{ strtoupper(str_replace('_info', '', $row->type)) }}</td>
              <td>{{ $row->created_at->diffforHumans() }}</td>
              <td><a href="{{ route('admin.payout.destroy',$row->id) }}" class="btn btn-danger btn-sm cancel"><i class="fas fa-trash"></i></a></td>
            </tr>
            @endforeach
          </tbody>
          <tfoot>
            <tr>
               <th>{{ __('User Id') }}</th>
              <th>{{ __('Method Name') }}</th>
                                        
              <th>{{ __('Created At') }}</th>
              <th>{{ __('Action') }}</th>
            </tr>
          </tfoot>
        </table>
        {{ $accounts->links() }}
      </div>
    </div>
  </div>
</div>
</div>
@endsection
