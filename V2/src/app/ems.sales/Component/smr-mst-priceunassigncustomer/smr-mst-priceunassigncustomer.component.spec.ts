import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstPriceunassigncustomerComponent } from './smr-mst-priceunassigncustomer.component';

describe('SmrMstPriceunassigncustomerComponent', () => {
  let component: SmrMstPriceunassigncustomerComponent;
  let fixture: ComponentFixture<SmrMstPriceunassigncustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstPriceunassigncustomerComponent]
    });
    fixture = TestBed.createComponent(SmrMstPriceunassigncustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
