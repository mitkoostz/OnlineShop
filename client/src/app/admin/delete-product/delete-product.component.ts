import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-delete-product',
  templateUrl: './delete-product.component.html',
  styleUrls: ['./delete-product.component.scss']
})
export class DeleteProductComponent implements OnInit {
  baseUrl = environment.apiUrl;
  id: number;
  name: string;
  description: string;
  price: number;
  pictureUrl: string;
  productType: string;
  productGenderBase: string;
  constructor(public bsModalRef: BsModalRef,private http: HttpClient,private toastr: ToastrService) { }

  ngOnInit(): void {
  }

    deleteProduct(id: number){
      let httpParams = new HttpParams().set('id', id.toString());
      
      let options = { params: httpParams };
    

     this.http.delete(this.baseUrl + "admin/deleteproduct",options).subscribe(
      (response) => this.toastr.success("Product is successfully DELETED!") ,
      (error) => this.toastr.error(error)
    )
  }

}
