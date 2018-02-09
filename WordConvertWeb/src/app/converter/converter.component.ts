import { Component, OnInit } from '@angular/core';
import { Profile } from '../shared/profile';
import { WordconverterService } from '../wordconverter.service';

@Component({
  selector: 'pm-converter',
  templateUrl: './converter.component.html',
  styleUrls: ['./converter.component.css']
})
export class ConverterComponent implements OnInit {
  profile: Profile = {
    name: 'Windstorm',
    no: 123.44,
    noString: ''
  };

  outProfile: Profile;

  constructor(private wordConverterService: WordconverterService) { }

  ngOnInit() {
  }

  onShow() : void{
    this.profile.name = "Li Meng Han";
    this.profile.noString = this.profile.no + "one two three";
    this.convertNumber();
  }

  convertNumber() : void {
    this.wordConverterService.convertNumber(this.profile)
      .subscribe(outprofile => {
        this.outProfile = outprofile;
        this.outProfile.name = this.profile.name;
      });
  }
}
