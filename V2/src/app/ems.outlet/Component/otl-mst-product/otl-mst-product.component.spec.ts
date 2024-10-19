import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstProductComponent } from './otl-mst-product.component';

describe('OtlMstProductComponent', () => {
  let component: OtlMstProductComponent;
  let fixture: ComponentFixture<OtlMstProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstProductComponent]
    });
    fixture = TestBed.createComponent(OtlMstProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
