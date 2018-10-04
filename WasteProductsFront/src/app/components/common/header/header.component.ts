import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  btnClick= function () {
    this.router.navigateByUrl('/products/all-to-list');
};

  ngOnInit() {
    
  }

}
