import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnQuoteeditnewComponent } from './smr-trn-quoteeditnew.component';

describe('SmrTrnQuoteeditnewComponent', () => {
  let component: SmrTrnQuoteeditnewComponent;
  let fixture: ComponentFixture<SmrTrnQuoteeditnewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnQuoteeditnewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnQuoteeditnewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
