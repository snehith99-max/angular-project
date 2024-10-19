import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnDebitnoteViewComponent } from './pbl-trn-debitnote-view.component';

describe('PblTrnDebitnoteViewComponent', () => {
  let component: PblTrnDebitnoteViewComponent;
  let fixture: ComponentFixture<PblTrnDebitnoteViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnDebitnoteViewComponent]
    });
    fixture = TestBed.createComponent(PblTrnDebitnoteViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
