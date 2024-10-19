import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstAssignProductComponent } from './otl-mst-assign-product.component';

describe('OtlMstAssignProductComponent', () => {
  let component: OtlMstAssignProductComponent;
  let fixture: ComponentFixture<OtlMstAssignProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstAssignProductComponent]
    });
    fixture = TestBed.createComponent(OtlMstAssignProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
