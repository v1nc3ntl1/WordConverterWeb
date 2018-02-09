import { Component, OnInit,  Input } from '@angular/core';
import { Profile } from '../shared/profile';

@Component({
  selector: 'pm-profile-output',
  templateUrl: './profile-output.component.html',
  styleUrls: ['./profile-output.component.css']
})
export class ProfileOutputComponent implements OnInit {

  @Input() profile: Profile;

  constructor() { }

  ngOnInit() {
  }

}
