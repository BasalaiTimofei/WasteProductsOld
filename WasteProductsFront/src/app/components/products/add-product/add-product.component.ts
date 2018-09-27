import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
selectedFile: File = null;

onFileSelected(event){
  this.selectedFile = <File>event.target.files[1];
}

onUpload(){

const fd = new FormData;
fd.append('image', this.selectedFile, this.selectedFile.name);
this.http.post('redirect to barcode parser', fd)
.subscribe(res => {console.log(res);
}
)};

isHidden = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  enableAdd = true;

  turnedOffWhile(){
  }
}