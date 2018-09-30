import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Product } from '../../../models/groups/Group';
import { ProductService } from '../../../services/product/product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

constructor(private http: HttpClient, private productService: ProductService) { }

selectedFile: File = null;

isHidden = false;

enableAdd = true;

onFileSelected(event) {
  this.selectedFile = <File>event.target.files[1];
}

onUpload(rating, descrText) {
  if (this.selectedFile !== null) {
    const fd = new FormData;
    fd.append('image', this.selectedFile, this.selectedFile.name);
    const url = `${environment.apiHostUrl}/api/products/`;

    this.http.post(url, fd)
    .subscribe(res => {
    const uploadResult = <IBarcodeUploadResult>res;

      if (uploadResult.product !== null) {
        this.productService.addProductDescription(Number(rating), descrText, uploadResult.product.Id);
      }
    },
     err => console.log(err));
  }
}

  ngOnInit() {
  }

  turnedOffWhile() {
  }
}

interface IBarcodeUploadResult {
  urlOfCreatedProduct: string;
  product: Product;
}
