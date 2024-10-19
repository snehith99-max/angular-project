import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstPriceunassignproductComponent } from './smr-mst-priceunassignproduct.component';

describe('SmrMstPriceunassignproductComponent', () => {
  let component: SmrMstPriceunassignproductComponent;
  let fixture: ComponentFixture<SmrMstPriceunassignproductComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstPriceunassignproductComponent]
    });
    fixture = TestBed.createComponent(SmrMstPriceunassignproductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
