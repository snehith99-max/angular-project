import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstAmendproductComponent } from './otl-mst-amendproduct.component';

describe('OtlMstAmendproductComponent', () => {
  let component: OtlMstAmendproductComponent;
  let fixture: ComponentFixture<OtlMstAmendproductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstAmendproductComponent]
    });
    fixture = TestBed.createComponent(OtlMstAmendproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
