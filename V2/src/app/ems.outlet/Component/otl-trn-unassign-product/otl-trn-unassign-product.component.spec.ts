import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlTrnUnassignProductComponent } from './otl-trn-unassign-product.component';

describe('OtlTrnUnassignProductComponent', () => {
  let component: OtlTrnUnassignProductComponent;
  let fixture: ComponentFixture<OtlTrnUnassignProductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlTrnUnassignProductComponent]
    });
    fixture = TestBed.createComponent(OtlTrnUnassignProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
