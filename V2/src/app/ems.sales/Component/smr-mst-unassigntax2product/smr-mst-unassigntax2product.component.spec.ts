import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrMstUnassigntax2productComponent } from './smr-mst-unassigntax2product.component';

describe('SmrMstUnassigntax2productComponent', () => {
  let component: SmrMstUnassigntax2productComponent;
  let fixture: ComponentFixture<SmrMstUnassigntax2productComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrMstUnassigntax2productComponent]
    });
    fixture = TestBed.createComponent(SmrMstUnassigntax2productComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
