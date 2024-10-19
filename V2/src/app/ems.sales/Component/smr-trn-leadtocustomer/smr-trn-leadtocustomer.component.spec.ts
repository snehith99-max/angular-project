import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnLeadtocustomerComponent } from './smr-trn-leadtocustomer.component';

describe('SmrTrnLeadtocustomerComponent', () => {
  let component: SmrTrnLeadtocustomerComponent;
  let fixture: ComponentFixture<SmrTrnLeadtocustomerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnLeadtocustomerComponent]
    });
    fixture = TestBed.createComponent(SmrTrnLeadtocustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
