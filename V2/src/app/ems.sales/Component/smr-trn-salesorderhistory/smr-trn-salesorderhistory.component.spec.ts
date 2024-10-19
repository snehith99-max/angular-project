import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnSalesorderhistoryComponent } from './smr-trn-salesorderhistory.component';

describe('SmrTrnSalesorderhistoryComponent', () => {
  let component: SmrTrnSalesorderhistoryComponent;
  let fixture: ComponentFixture<SmrTrnSalesorderhistoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnSalesorderhistoryComponent]
    });
    fixture = TestBed.createComponent(SmrTrnSalesorderhistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
