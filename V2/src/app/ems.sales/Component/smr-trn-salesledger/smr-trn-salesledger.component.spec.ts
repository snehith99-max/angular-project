import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesledgerComponent } from './smr-trn-salesledger.component';

describe('SmrTrnSalesledgerComponent', () => {
  let component: SmrTrnSalesledgerComponent;
  let fixture: ComponentFixture<SmrTrnSalesledgerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesledgerComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesledgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
