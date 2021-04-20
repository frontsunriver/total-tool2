@component('mail::message')
# New Message

Name: {{ $data['name'] }}<br>
Email: {{ $data['email'] }}<br>
Subject: {{ $data['subject'] }}<br>
Message: {{ $data['message'] }}

Thanks,<br>
{{ config('app.name') }}
@endcomponent
