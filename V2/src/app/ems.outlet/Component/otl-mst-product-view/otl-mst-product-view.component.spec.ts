import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtlMstProductViewComponent } from './otl-mst-product-view.component';

describe('OtlMstProductViewComponent', () => {
  let component: OtlMstProductViewComponent;
  let fixture: ComponentFixture<OtlMstProductViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OtlMstProductViewComponent]
    });
    fixture = TestBed.createComponent(OtlMstProductViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
